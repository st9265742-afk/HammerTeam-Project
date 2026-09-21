using UnityEngine;
using UnityEngine.UI;

public class WeaponUI : MonoBehaviour
{
    [SerializeField] private PlayerWeaponController weaponController;
    [SerializeField] private Text ammoText;

    private void Update()
    {
        Weapon weapon = weaponController.CurrentWeapon;

        ammoText.text = $"{weapon.CurrentAmmo} / {weapon.ReserveAmmo}";
    }
}