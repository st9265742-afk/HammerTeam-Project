using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float speed = 3f;

    [Header("Vision")]
    [SerializeField] private float viewDistance = 15f;
    [SerializeField, Range(10f, 360f)] private float viewAngle = 120f;
    [SerializeField] private float eyeHeight = 1f;
    [SerializeField] private LayerMask obstacleMask;

    [Header("Loot")]
    [SerializeField] private LootTable lootTable;

    private float _health;
    private PlayerHealth _player;
    private Collider _playerCollider;
    private NavMeshAgent _agent;
    private EnemyAttack _attack;
    private bool _isAlerted;
    private Vector3 _lastKnownPosition;

    public float Health => _health;
    public float MaxHealth => maxHealth;
    public float Speed => speed;
    public bool SeesPlayer { get; private set; }
    public Vector3 PlayerCenter => _playerCollider.bounds.center;

    // Нерухомі вороги (турель) не мають NavMeshAgent
    private bool CanMove => _agent != null;

    private void Awake()
    {
        _health = maxHealth;
        _player = FindAnyObjectByType<PlayerHealth>();
        _playerCollider = _player.GetComponent<Collider>();
        _attack = GetComponent<EnemyAttack>();
        _agent = GetComponent<NavMeshAgent>();

        if (CanMove)
        {
            _agent.speed = speed;
            _agent.stoppingDistance = 0.5f;
        }
    }

    private void Update()
    {
        if (_player == null || _player.Health <= 0)
        {
            StopMoving();
            return;
        }

        SeesPlayer = CanSeePlayer();

        if (SeesPlayer)
        {
            _isAlerted = true;
            _lastKnownPosition = _player.transform.position;
        }

        if (!_isAlerted)
        {
            return;
        }

        float distance = Vector3.Distance(transform.position, _player.transform.position);

        if (SeesPlayer && distance <= _attack.Range && _attack.CanAttack(_player))
        {
            StopMoving();
            FacePlayer();
            _attack.TryAttack(_player);
        }
        else if (CanMove)
        {
            _agent.SetDestination(_lastKnownPosition);

            if (!SeesPlayer && !_agent.pathPending && _agent.remainingDistance <= _agent.stoppingDistance)
            {
                _isAlerted = false;
            }
        }
    }

    private bool CanSeePlayer()
    {
        Vector3 eyes = transform.position + Vector3.up * eyeHeight;
        Vector3 direction = PlayerCenter - eyes;
        float distance = direction.magnitude;

        if (distance > viewDistance)
        {
            return false;
        }

        if (!_isAlerted && Vector3.Angle(transform.forward, direction) > viewAngle * 0.5f)
        {
            return false;
        }

        return !Physics.Raycast(eyes, direction.normalized, distance, obstacleMask);
    }

    private void StopMoving()
    {
        if (CanMove)
        {
            _agent.ResetPath();
        }
    }

    private void FacePlayer()
    {
        if (!CanMove)
        {
            return;
        }

        Vector3 flat = _player.transform.position - transform.position;
        flat.y = 0f;

        if (flat.sqrMagnitude < 0.01f)
        {
            return;
        }

        Quaternion target = Quaternion.LookRotation(flat);
        transform.rotation = Quaternion.Slerp(transform.rotation, target, 10f * Time.deltaTime);
    }

    public virtual void TakeDamage(float damageAmount)
    {
        if (_health <= 0)
        {
            return;
        }

        _health = Mathf.Max(_health - damageAmount, 0f);

        _isAlerted = true;
        _lastKnownPosition = _player.transform.position;

        if (_health <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        lootTable?.TrySpawnLoot(transform.position);
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, viewDistance);
    }
}