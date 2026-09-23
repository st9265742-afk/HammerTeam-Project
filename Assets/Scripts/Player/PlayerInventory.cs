using System;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    private PlayerHealth _health;
    private PlayerKeyCards _keyCards;

    public InventoryState State => GameSession.Inventory;

    public event Action<WeaponState> WeaponAdded;

    private void Awake()
    {
        _health = GetComponent<PlayerHealth>();
        _keyCards = GetComponent<PlayerKeyCards>();
    }

    public void AddWeapon(WeaponData data, int ammo)
    {
        if (State.FindWeapon(data) == null)
        {
            WeaponState state = State.AddWeapon(data);
            WeaponAdded?.Invoke(state);
        }

        AddAmmo(data.AmmoType, ammo);
    }

    public void AddAmmo(AmmoType type, int amount) => State.Ammo.Add(type, amount);

    public void AddArmor(float amount) => _health.AddArmor(amount);

    public void Heal(float amount) => _health.Heal(amount);

    public void AddKeyCard(KeyCardType type) => _keyCards.Add(type);
}