using UnityEngine;

public class PlayerHud : MonoBehaviour
{
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private InputManager inputManager;
    [SerializeField] private DeathScreen deathScreen;

    private void Awake()
    {
        deathScreen.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        playerHealth.Died += OnPlayerDied;
    }

    private void OnDisable()
    {
        playerHealth.Died -= OnPlayerDied;
    }

    private void OnPlayerDied()
    {
        inputManager.enabled = false;

        foreach (Transform child in transform)
        {
            if (child != deathScreen.transform)
            {
                child.gameObject.SetActive(false);
            }
        }

        deathScreen.Show();
    }
}