using UnityEngine;

public class TurretHead : MonoBehaviour
{
    [SerializeField] private Enemy enemy;
    [SerializeField] private Transform head;

    private void Update()
    {
        if (!enemy.SeesPlayer)
        {
            return;
        }

        head.rotation = Quaternion.LookRotation(enemy.PlayerCenter - head.position);
    }
}