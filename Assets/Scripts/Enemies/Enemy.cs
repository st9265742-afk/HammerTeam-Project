using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float speed = 3f;
    [SerializeField] private float damage = 10f;

    [Header("Attack")]
    [SerializeField] private float attackRange = 1.5f;
    [SerializeField] private float attackCooldown = 1f;

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
    private float _nextAttackTime;
    private bool _isAlerted;
    private Vector3 _lastKnownPosition;
    protected LayerMask ObstacleMask => obstacleMask;
    protected Vector3 PlayerCenter => _playerCollider.bounds.center;

    public float Health => _health;
    public float MaxHealth => maxHealth;
    public float Speed => speed;
    public float Damage => damage;

    private void Awake()
    {
        _health = maxHealth;
        _player = FindAnyObjectByType<PlayerHealth>();
        _playerCollider = _player.GetComponent<Collider>();

        _agent = GetComponent<NavMeshAgent>();
        _agent.speed = speed;
        _agent.stoppingDistance = 0.5f;
    }

    private void Update()
    {
        if (_player == null || _player.Health <= 0)
        {
            _agent.ResetPath();
            return;
        }

        bool canSeePlayer = CanSeePlayer();

        if (canSeePlayer)
        {
            _isAlerted = true;
            _lastKnownPosition = _player.transform.position;
        }

        if (!_isAlerted)
        {
            return;
        }

        float distance = Vector3.Distance(transform.position, _player.transform.position);

        if (canSeePlayer && distance <= attackRange && CanAttack(_player))
        {
            _agent.ResetPath();
            FacePlayer();
            TryAttack();
        }
        else
        {
            _agent.SetDestination(_lastKnownPosition);

            if (!canSeePlayer && !_agent.pathPending && _agent.remainingDistance <= _agent.stoppingDistance)
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

    protected virtual bool CanAttack(PlayerHealth target) => true;

    private void FacePlayer()
    {
        Vector3 flat = _player.transform.position - transform.position;
        flat.y = 0f;

        if (flat.sqrMagnitude < 0.01f)
        {
            return;
        }

        Quaternion target = Quaternion.LookRotation(flat);
        transform.rotation = Quaternion.Slerp(transform.rotation, target, 10f * Time.deltaTime);
    }

    private void TryAttack()
    {
        if (Time.time < _nextAttackTime)
        {
            return;
        }

        _nextAttackTime = Time.time + attackCooldown;
        Attack(_player);
    }

    protected virtual void Attack(PlayerHealth target)
    {
        target.TakeDamage(damage);
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