using UnityEngine;

public class HitscanWeapon : Weapon
{
    [Header("Hitscan Settings")]
    [SerializeField] private float range = 100f;
    [SerializeField] private int pellets = 1;
    [SerializeField] private float spread = 0f;

    protected override void Fire()
    {
        for (int i = 0; i < pellets; i++)
        {
            Vector3 direction = GetSpreadDirection(spread);

            if (Physics.Raycast(PlayerCamera.transform.position, direction, out RaycastHit hit, range))
            {
                ApplyHit(hit);
            }
        }
    }
}