using UnityEngine;

public class ArmorPickup : ItemPickup
{
    [SerializeField] private float amount = 25f;

    protected override void Collect(PlayerInventory inventory)
    {
        inventory.AddArmor(amount);
    }
}