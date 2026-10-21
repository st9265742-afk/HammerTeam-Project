using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float speed = 3f;
    [SerializeField] private float damage = 10f;

    [Header("Attack")]
    [SerializeField] private float attackRange = 1.5f;
    [SerializeField] private float attackCooldown = 1f;

    [Header("Loot")]
    [SerializeField] private LootTable lootTable;

    private float _health;
    private PlayerHealth _player;
    private float _nextAttackTime;

    public float Health => _health;
    public float MaxHealth => maxHealth;
    public float Speed => speed;
    public float Damage => damage;

    private void Awake()
    {
        _health = maxHealth;
        _player = FindAnyObjectByType<PlayerHealth>();
    }

    private void Update()
    {
        if (_player == null || _player.Health <= 0)
        {
            return;
        }

        if (Time.time < _nextAttackTime)
        {
            return;
        }

        if (Vector3.Distance(transform.position, _player.transform.position) > attackRange)
        {
            return;
        }

        _nextAttackTime = Time.time + attackCooldown;
        Attack(_player);
    }

    protected virtual void Attack(PlayerHealth target)
    {
        target.TakeDamage(damage);
        Debug.Log($"{gameObject.name} hit player: HP {target.Health}, armor {target.Armor}");
    }

    public void TakeDamage(float damageAmount)
    {
        if (_health <= 0)
        {
            return;
        }

        _health -= damageAmount;
        _health = Mathf.Max(_health, 0);
        Debug.Log($"{gameObject.name} HP: {_health}");

        if (_health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log($"{gameObject.name} died.");
        lootTable?.TrySpawnLoot(transform.position);
        Destroy(gameObject);
    }
}