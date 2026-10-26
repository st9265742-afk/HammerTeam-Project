using UnityEngine;

public class MeleeAttack : EnemyAttack
{
    [SerializeField] private float damage = 10f;

    protected override void Attack(PlayerHealth target)
    {
        target.TakeDamage(damage);
    }
}