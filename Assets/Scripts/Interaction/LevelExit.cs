using UnityEngine;

[RequireComponent(typeof(Collider))]
public class LevelExit : MonoBehaviour
{
    [SerializeField, SceneName] private string nextScene;
    [SerializeField] private PlayerHud hud;

    private void Awake()
    {
        GetComponent<Collider>().isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerHealth health = other.GetComponentInParent<PlayerHealth>();

        if (health == null)
        {
            return;
        }

        health.GetComponentInChildren<PlayerHud>(true).ShowLevelComplete(nextScene);
    }
}