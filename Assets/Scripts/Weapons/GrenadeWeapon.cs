using UnityEngine;

public class GrenadeWeapon : Weapon
{
    [SerializeField] private GameObject grenadePrefab;
    [SerializeField] private Transform throwPoint;
    [SerializeField] private float throwForce = 15f;

    protected override void Fire()
    {
        if (grenadePrefab == null) return;

        Vector3 spawnPosition = throwPoint != null
            ? throwPoint.position
            : PlayerCamera.transform.position + PlayerCamera.transform.forward * 0.5f;

        GameObject grenade = Instantiate(grenadePrefab, spawnPosition, PlayerCamera.transform.rotation);

        if (grenade.TryGetComponent<Rigidbody>(out var rb))
        {
            rb.AddForce(PlayerCamera.transform.forward * throwForce, ForceMode.Impulse);
        }
    }
}