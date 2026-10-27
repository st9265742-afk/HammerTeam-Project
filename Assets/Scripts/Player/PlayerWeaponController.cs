using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerInventory))]
public class PlayerWeaponController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Camera playerCamera;
    [SerializeField] private Transform weaponHolder;

    [Header("Starting loadout")]
    [SerializeField] private WeaponData startingWeapon;
    [SerializeField] private int startingAmmo = 24;

    private PlayerInventory _inventory;
    private readonly Dictionary<WeaponState, Weapon> _instances = new Dictionary<WeaponState, Weapon>();
    private Weapon _currentWeapon;

    public Weapon CurrentWeapon => _currentWeapon;

    private void Awake()
    {
        _inventory = GetComponent<PlayerInventory>();
    }

    private void OnEnable()
    {
        _inventory.WeaponAdded += HandleWeaponAdded;
    }

    private void OnDisable()
    {
        _inventory.WeaponAdded -= HandleWeaponAdded;
    }

    private void Start()
    {
        foreach (WeaponState state in _inventory.State.Weapons)
        {
            SpawnWeapon(state);
        }

        Equip(_inventory.State.CurrentWeaponIndex);

        if (_inventory.State.Weapons.Count == 0 && startingWeapon != null)
        {
            _inventory.AddWeapon(startingWeapon, startingAmmo);
        }
    }

    public void SwitchWeapon(int direction)
    {
        int count = _inventory.State.Weapons.Count;

        if (count <= 1)
        {
            return;
        }

        int next = (_inventory.State.CurrentWeaponIndex + direction + count) % count;
        Equip(next);
    }

    public void ProcessFire(bool fire, bool alternateFire)
    {
        if (_currentWeapon == null)
        {
            return;
        }

        if (fire)
        {
            _currentWeapon.TryFire();
        }

        if (alternateFire)
        {
            _currentWeapon.TryAlternateFire();
        }
    }

    public void Reload()
    {
        if (_currentWeapon != null)
        {
            _currentWeapon.Reload();
        }
    }

    private void HandleWeaponAdded(WeaponState state)
    {
        SpawnWeapon(state);

        Equip(_inventory.State.Weapons.Count - 1);
    }

    private void SpawnWeapon(WeaponState state)
    {
        Weapon weapon = Instantiate(state.Data.Prefab, weaponHolder);
        weapon.Initialize(state, _inventory.State.Ammo, playerCamera);
        weapon.gameObject.SetActive(false);

        _instances.Add(state, weapon);
    }

    private void Equip(int index)
    {
        if (index < 0 || index >= _inventory.State.Weapons.Count)
        {
            return;
        }

        Weapon weapon = _instances[_inventory.State.Weapons[index]];

        if (weapon == _currentWeapon)
        {
            return;
        }

        if (_currentWeapon != null)
        {
            _currentWeapon.gameObject.SetActive(false);
        }

        _currentWeapon = weapon;
        _currentWeapon.gameObject.SetActive(true);
        _inventory.State.CurrentWeaponIndex = index;
    }
}