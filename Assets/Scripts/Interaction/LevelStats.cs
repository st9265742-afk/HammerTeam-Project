using UnityEngine;

public class LevelStats : MonoBehaviour
{
    public int Kills { get; private set; }
    public float Elapsed => Time.timeSinceLevelLoad;

    private void OnEnable() => Enemy.Killed += OnEnemyKilled;
    private void OnDisable() => Enemy.Killed -= OnEnemyKilled;

    private void OnEnemyKilled() => Kills++;
}