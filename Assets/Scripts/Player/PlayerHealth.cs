using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private float maxHealth = 100f;

    [Header("Overheal")]
    [SerializeField] private float overhealCap = 200f;
    [SerializeField] private float overhealDecayPerSecond = 1f;

    [Header("Armor")]
    [SerializeField] private float maxArmor = 100f;
    [SerializeField, Range(0f, 1f)] private float armorAbsorption = 0.5f;

    private float _health;
    private float _armor;
    private float _overhealDecayTimer;

    public float Health => _health;
    public float MaxHealth => maxHealth;
    public float Armor => _armor;
    public float MaxArmor => maxArmor;

    public event Action Died;
    public event Action Changed;

    private void Awake()
    {
        _health = GameSession.Inventory.Health ?? maxHealth;
        _armor = GameSession.Inventory.Armor;
    }

    private void Update()
    {
        if (_health <= maxHealth)
        {
            _overhealDecayTimer = 0f;
            return;
        }

        _overhealDecayTimer += Time.deltaTime;

        while (_overhealDecayTimer >= 1f && _health > maxHealth)
        {
            _overhealDecayTimer -= 1f;
            _health = Mathf.Max(_health - overhealDecayPerSecond, maxHealth);
            Changed?.Invoke();
        }
    }

    public void TakeDamage(float amount)
    {
        if (_health <= 0 || amount <= 0)
        {
            return;
        }

        if (_armor > 0)
        {
            float absorbed = Mathf.Min(_armor, amount * armorAbsorption);
            _armor -= absorbed;
            amount -= absorbed;
        }

        _health = Mathf.Max(_health - amount, 0);
        Changed?.Invoke();

        if (_health <= 0)
        {
            Died?.Invoke();
        }
    }

    public void Heal(float amount)
    {
        _health = Mathf.Min(_health + amount, overhealCap);
        Changed?.Invoke();
    }

    public void AddArmor(float amount)
    {
        _armor = Mathf.Min(_armor + amount, maxArmor);
        Changed?.Invoke();
    }

    public void SaveToSession()
    {
        GameSession.Inventory.Health = _health;
        GameSession.Inventory.Armor = _armor;
    }
}