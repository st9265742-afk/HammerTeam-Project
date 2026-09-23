using UnityEngine;

public class WeaponPickup : Pickup
{
    [SerializeField] private WeaponData weapon;
    [SerializeField] private int ammo = 12;

    public override string InteractionPrompt => $"Pick up {weapon.DisplayName}";

    protected override bool TryPickUp(PlayerInventory inventory)
    {
        inventory.AddWeapon(weapon, ammo);
        return true;
    }
}