using UnityEngine;

public abstract class EnemyAttack : MonoBehaviour
{
    [SerializeField] private float range = 1.5f;
    [SerializeField] private float cooldown = 1f;

    private float _nextAttackTime;

    public float Range => range;

    // Чи може атакувати зараз (наприклад, дуло не всередині стіни)
    public virtual bool CanAttack(PlayerHealth target) => true;

    public void TryAttack(PlayerHealth target)
    {
        if (Time.time < _nextAttackTime)
        {
            return;
        }

        _nextAttackTime = Time.time + cooldown;
        Attack(target);
    }

    protected abstract void Attack(PlayerHealth target);
}