using UnityEngine;
using UnityEngine.UI;

public class WeaponUI : MonoBehaviour
{
    [SerializeField] private Weapon weapon;
    [SerializeField] private Text ammoText;

    private void Update()
    {
        ammoText.text = $"{weapon.CurrentAmmo} / {weapon.ReserveAmmo}";
    }
}