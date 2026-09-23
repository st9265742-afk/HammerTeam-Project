using UnityEngine;

[CreateAssetMenu(menuName = "Game/Weapon Data", fileName = "WeaponData")]
public class WeaponData : ScriptableObject
{
    [SerializeField] private string displayName;
    [SerializeField] private Weapon prefab;
    [SerializeField] private AmmoType ammoType;
    [SerializeField] private int magazineSize = 6;

    public string DisplayName => displayName;
    public Weapon Prefab => prefab;
    public AmmoType AmmoType => ammoType;
    public int MagazineSize => magazineSize;
}