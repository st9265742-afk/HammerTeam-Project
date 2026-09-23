using UnityEngine;

public class Revolver : Weapon
{
    [SerializeField] private float range = 100f;

    protected override void Fire()
    {
        Ray ray = new Ray(PlayerCamera.transform.position, PlayerCamera.transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, range))
        {
            ApplyHit(hit);
        }
    }
}