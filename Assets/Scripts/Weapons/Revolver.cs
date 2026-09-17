using UnityEngine;

public class Revolver : Weapon
{
    [SerializeField] private Camera playerCamera;
    [SerializeField] private float range = 100f;

    protected override void Fire()
    {
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, range))
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