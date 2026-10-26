using UnityEngine;

public class Soldier : Enemy
{
    [Header("Shooting")]
    [SerializeField] private Transform muzzle;
    [SerializeField] private LayerMask shotMask;
    [SerializeField] private float shotDamage = 8f;
    [SerializeField] private int pellets = 1;
    [SerializeField, Range(0f, 0.5f)] private float spread = 0.05f;

    [Header("Melee")]
    [SerializeField] private float meleeRange = 2f;

    protected override void Attack(PlayerHealth target)
    {
        float distance = Vector3.Distance(transform.position, target.transform.position);

        if (distance <= meleeRange)
        {
            base.Attack(target);
            return;
        }

        Shoot(target);
    }

    protected override bool CanAttack(PlayerHealth target)
    {
        if (Vector3.Distance(transform.position, target.transform.position) <= meleeRange)
        {
            return true;
        }

        if (Physics.CheckSphere(muzzle.position, 0.1f, ObstacleMask))
        {
            return false;
        }

        return !Physics.Linecast(muzzle.position, PlayerCenter, ObstacleMask);
    }

    private void Shoot(PlayerHealth target)
    {
        Vector3 aimDirection = (PlayerCenter - muzzle.position).normalized;

        for (int i = 0; i < pellets; i++)
        {
            Vector3 direction = (aimDirection + Random.insideUnitSphere * spread).normalized;
            Debug.DrawRay(muzzle.position, direction * 20f, Color.red, 0.2f);

            if (Physics.Raycast(muzzle.position, direction, out RaycastHit hit, 100f, shotMask))
            {
                PlayerHealth player = hit.collider.GetComponentInParent<PlayerHealth>();
                if (player != null)
                {
                    player.TakeDamage(shotDamage);
                }
            }
        }
    }
}