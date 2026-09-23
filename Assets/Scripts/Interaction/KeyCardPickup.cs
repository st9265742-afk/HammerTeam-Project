using UnityEngine;

public class KeyCardPickup : ItemPickup
{
    [SerializeField] private KeyCardType type;

    protected override void Collect(PlayerInventory inventory)
    {
        inventory.AddKeyCard(type);
    }
}