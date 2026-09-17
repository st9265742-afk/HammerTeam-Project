using UnityEngine;

public class PlayerWeaponController : MonoBehaviour
{
    [SerializeField] private Weapon currentWeapon;

    public void ProcessFire(bool fire, bool alternateFire)
    {
        if (fire)
        {
            currentWeapon.TryFire();
        }

        if (alternateFire)
        {
            currentWeapon.AlternateFire();
        }
    }

    public void Reload()
    {
        currentWeapon.Reload();
    }
}