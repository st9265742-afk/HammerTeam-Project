public class WeaponState
{
    public WeaponData Data { get; }
    public int MagazineAmmo { get; set; }

    public WeaponState(WeaponData data)
    {
        Data = data;
        MagazineAmmo = data.MagazineSize;
    }
}