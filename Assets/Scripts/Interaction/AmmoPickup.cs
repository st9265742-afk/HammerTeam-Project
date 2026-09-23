using UnityEngine;

public class AmmoPickup : ItemPickup
{
    [SerializeField] private AmmoType ammoType;
    [SerializeField] private int amount = 12;

    protected override void Collect(PlayerInventory inventory)
    {
        inventory.AddAmmo(ammoType, amount);
    }
}