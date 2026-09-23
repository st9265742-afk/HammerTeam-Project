using UnityEngine;

public class MedkitPickup : ItemPickup
{
    [SerializeField] private float healAmount = 25f;

    protected override void Collect(PlayerInventory inventory)
    {
        inventory.Heal(healAmount);
    }
}