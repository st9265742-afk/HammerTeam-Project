using UnityEngine;

public class PlayerHud : MonoBehaviour
{
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private InputManager inputManager;
    [SerializeField] private DeathScreen deathScreen;
    [SerializeField] private AdrenalinePanel adrenalinePanel;
    [SerializeField] private LevelCompleteScreen levelCompleteScreen;

    private void Awake()
    {
        deathScreen.gameObject.SetActive(false);
        levelCompleteScreen.gameObject.SetActive(false);
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
        ShowOnly(deathScreen.transform);
        deathScreen.Show();
    }

    public void ShowLevelComplete(string nextScene)
    {
        ShowOnly(levelCompleteScreen.transform);
        levelCompleteScreen.Show(nextScene);
    }

    private void ShowOnly(Transform screen)
    {
        inputManager.enabled = false;

        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(child == screen);
        }
    }
}