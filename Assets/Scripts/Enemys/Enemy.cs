using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float speed = 3f;
    [SerializeField] private float damage = 10f;

    private float _health;

    public float Health => _health;
    public float MaxHealth => maxHealth;
    public float Speed => speed;
    public float Damage => damage;

    private void Awake()
    {
        _health = maxHealth;
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
        Destroy(gameObject);
    }
}