using UnityEngine;

public class GrenadeWeapon : Weapon
{
    [SerializeField] private GameObject grenadePrefab;
    [SerializeField] private Transform throwPoint;
    [SerializeField] private float throwForce = 15f;
    [SerializeField] private float weakThrowForce = 7f;

    protected override bool HasAlternateFire => true;

    private void Awake()
    {
        if (grenadePrefab == null)
        {
            Debug.LogError($"{name}: grenadePrefab is not assigned", this);
        }
    }

    protected override void Fire() => Throw(throwForce);

    protected override void AlternateFire() => Throw(weakThrowForce);

    private void Throw(float force)
    {
        Vector3 spawnPosition = throwPoint != null
            ? throwPoint.position
            : PlayerCamera.transform.position + PlayerCamera.transform.forward * 0.5f;

        GameObject grenade = Instantiate(grenadePrefab, spawnPosition, PlayerCamera.transform.rotation);

        if (grenade.TryGetComponent<Rigidbody>(out var rb))
        {
            rb.AddForce(PlayerCamera.transform.forward * force, ForceMode.Impulse);
        }
    }
}