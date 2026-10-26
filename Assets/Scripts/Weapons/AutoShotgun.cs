using UnityEngine;

public class AutoShotgun : Weapon
{
    [SerializeField] private float range = 40f;
    [SerializeField] private int pellets = 8;
    [SerializeField] private float spread = 5.5f;

    protected override void Fire()
    {
        for (int i = 0; i < pellets; i++)
        {
            Vector3 direction = GetSpreadDirection();

            if (Physics.Raycast(PlayerCamera.transform.position, direction, out RaycastHit hit, range))
            {
                ApplyHit(hit);
            }
        }
    }

    private Vector3 GetSpreadDirection()
    {
        Vector3 direction = PlayerCamera.transform.forward;

        direction += PlayerCamera.transform.right * Random.Range(-spread, spread) / 100f;
        direction += PlayerCamera.transform.up * Random.Range(-spread, spread) / 100f;

        return direction.normalized;
    }
}