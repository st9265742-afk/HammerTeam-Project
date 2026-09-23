using System.Collections.Generic;

public class InventoryState
{
    private readonly List<WeaponState> _weapons = new List<WeaponState>();

    public AmmoPool Ammo { get; } = new AmmoPool();
    public IReadOnlyList<WeaponState> Weapons => _weapons;
    public int CurrentWeaponIndex { get; set; }

    public WeaponState FindWeapon(WeaponData data) => _weapons.Find(w => w.Data == data);

    public WeaponState AddWeapon(WeaponData data)
    {
        WeaponState state = new WeaponState(data);
        _weapons.Add(state);
        return state;
    }
}