using UnityEngine;

public class Shotgun : Weapon
{
    [SerializeField] private Camera playerCamera;
    [SerializeField] private float range = 50f;
    [SerializeField] private int pellets = 6;
    [SerializeField] private float spread = 5f;

    protected override void Fire()
    {
        for (int i = 0; i < pellets; i++)
        {
            Vector3 direction = GetSpreadDirection();

            if (Physics.Raycast(
                    playerCamera.transform.position,
                    direction,
                    out RaycastHit hit,
                    range))
            {
                Debug.Log($"Hit: {hit.collider.name}");

                Enemy enemy = hit.collider.GetComponent<Enemy>();

                if (enemy != null)
                {
                    enemy.TakeDamage(damage);
                }
            }
        }
    }

    private Vector3 GetSpreadDirection()
    {
        Vector3 direction = playerCamera.transform.forward;

        direction += playerCamera.transform.right *
                     Random.Range(-spread, spread) / 100f;

        direction += playerCamera.transform.up *
                     Random.Range(-spread, spread) / 100f;

        return direction.normalized;
    }
}