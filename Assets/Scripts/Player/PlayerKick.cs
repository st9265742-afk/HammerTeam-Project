using UnityEngine;

public class PlayerKick : MonoBehaviour
{
    [SerializeField] private Camera playerCamera;
    [SerializeField] private float damage = 15f;
    [SerializeField] private float range = 2f;
    [SerializeField] private float cooldown = 0.6f;

    private float _nextKickTime;

    public void TryKick()
    {
        if (Time.time < _nextKickTime)
        {
            return;
        }

        _nextKickTime = Time.time + cooldown;

        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, range, ~0, QueryTriggerInteraction.Ignore))
        {
            Enemy enemy = hit.collider.GetComponentInParent<Enemy>();

            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
        }
    }
}