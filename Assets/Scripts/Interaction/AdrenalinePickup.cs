using UnityEngine;

public class AdrenalinePickup : ItemPickup
{
    [SerializeField] private float speedMultiplier = 1.5f;
    [SerializeField] private float duration = 8f;

    protected override void Collect(PlayerInventory inventory)
    {
        inventory.ApplyAdrenaline(speedMultiplier, duration);
    }
}