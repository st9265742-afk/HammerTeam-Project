using UnityEngine;

public class PlayerHud : MonoBehaviour
{
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private InputManager inputManager;
    [SerializeField] private DeathScreen deathScreen;
    [SerializeField] private AdrenalinePanel adrenalinePanel;

    private void Awake()
    {
        deathScreen.gameObject.SetActive(false);
        adrenalinePanel.SetVisible(false);
    }

    private void OnEnable()
    {
        playerHealth.Died += OnPlayerDied;
        playerMovement.SpeedBoostChanged += OnSpeedBoostChanged;
    }

    private void OnDisable()
    {
        playerHealth.Died -= OnPlayerDied;
        playerMovement.SpeedBoostChanged -= OnSpeedBoostChanged;
    }

    private void OnSpeedBoostChanged(bool isActive)
    {
        adrenalinePanel.SetVisible(isActive);
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