using UnityEngine;

public class SMG : Weapon
{
    [SerializeField] private float range = 80f;
    [SerializeField] private float spread = 2f;

    protected override void Fire()
    {
        Vector3 direction = GetSpreadDirection();

        if (Physics.Raycast(PlayerCamera.transform.position, direction, out RaycastHit hit, range))
        {
            ApplyHit(hit);
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