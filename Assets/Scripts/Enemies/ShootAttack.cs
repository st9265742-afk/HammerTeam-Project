using UnityEngine;

public class ShootAttack : EnemyAttack
{
    [Header("Shooting")]
    [SerializeField] private Transform muzzle;
    [SerializeField] private LayerMask obstacleMask; // стіни
    [SerializeField] private LayerMask shotMask;     // стіни + гравець
    [SerializeField] private float shotDamage = 8f;
    [SerializeField] private int pellets = 1;
    [SerializeField, Range(0f, 0.5f)] private float spread = 0.05f;

    [Header("Melee")]
    [SerializeField] private float meleeRange = 2f;
    [SerializeField] private float meleeDamage = 10f;

    private Enemy _enemy;

    private void Awake()
    {
        _enemy = GetComponent<Enemy>();
    }

    public override bool CanAttack(PlayerHealth target)
    {
        if (IsInMeleeRange(target))
        {
            return true;
        }

        if (Physics.CheckSphere(muzzle.position, 0.1f, obstacleMask))
        {
            return false;
        }

        return !Physics.Linecast(muzzle.position, _enemy.PlayerCenter, obstacleMask);
    }

    protected override void Attack(PlayerHealth target)
    {
        if (IsInMeleeRange(target))
        {
            target.TakeDamage(meleeDamage);
            return;
        }

        Shoot();
    }

    private bool IsInMeleeRange(PlayerHealth target)
    {
        return Vector3.Distance(transform.position, target.transform.position) <= meleeRange;
    }

    private void Shoot()
    {
        Vector3 aimDirection = (_enemy.PlayerCenter - muzzle.position).normalized;

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