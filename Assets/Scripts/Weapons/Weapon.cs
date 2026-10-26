using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    [Header("Weapon Settings")]
    [SerializeField] protected float damage = 10f;
    [SerializeField] protected float fireRate = 0.2f;

    private WeaponState _state;
    private AmmoPool _ammoPool;
    private float _nextFireTime;

    protected Camera PlayerCamera { get; private set; }

    public WeaponData Data => _state?.Data;
    public int CurrentAmmo => _state != null ? _state.MagazineAmmo : 0;
    public int ReserveAmmo => (_state != null && _ammoPool != null) ? _ammoPool.Get(_state.Data.AmmoType) : 0;

    public void Initialize(WeaponState state, AmmoPool ammoPool, Camera playerCamera)
    {
        _state = state;
        _ammoPool = ammoPool;
        PlayerCamera = playerCamera;
    }

    public void TryFire()
    {
        if (Time.time < _nextFireTime)
        {
            return;
        }

        if (_state == null || _state.MagazineAmmo <= 0)
        {
            return;
        }

        Fire();

        _state.MagazineAmmo--;
        _nextFireTime = Time.time + fireRate;
    }

    protected virtual void ApplyHit(RaycastHit hit)
    {
        Debug.Log($"Hit: {hit.collider.name}");

        Enemy enemy = hit.collider.GetComponent<Enemy>();

        if (enemy != null)
        {
            enemy.TakeDamage(damage);
        }
    }

    public virtual void AlternateFire()
    {
    }

    public virtual void Reload()
    {
        if (_state == null)
        {
            return;
        }

        int missing = _state.Data.MagazineSize - _state.MagazineAmmo;

        if (missing <= 0)
        {
            return;
        }

        _state.MagazineAmmo += _ammoPool.Take(_state.Data.AmmoType, missing);
    }

    protected abstract void Fire();
}