using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    [Header("Weapon")]
    [SerializeField] protected float damage = 10f;
    [SerializeField] protected float fireRate = 0.2f;

    [Header("Ammo")]
    [SerializeField] protected int magazineSize = 6;
    [SerializeField] protected int currentAmmo = 6;
    [SerializeField] protected int reserveAmmo = 24;

    private float _nextFireTime;

    public int CurrentAmmo => currentAmmo;
    public int ReserveAmmo => reserveAmmo;

    public void TryFire()
    {
        if (Time.time < _nextFireTime)
        {
            return;
        }

        if (currentAmmo <= 0)
        {
            return;
        }

        Fire();

        currentAmmo--;
        _nextFireTime = Time.time + fireRate;
    }

    public virtual void AlternateFire()
    {
    }

    public virtual void Reload()
    {

        if (currentAmmo >= magazineSize || reserveAmmo <= 0)
        {
            return;
        }

        int requiredAmmo = magazineSize - currentAmmo;
        int ammoToReload = Mathf.Min(requiredAmmo, reserveAmmo);

        currentAmmo += ammoToReload;
        reserveAmmo -= ammoToReload;
    }

    protected abstract void Fire();
}