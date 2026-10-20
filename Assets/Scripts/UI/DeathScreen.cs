using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DeathScreen : MonoBehaviour
{
    [SerializeField] private Button restartButton;

    private void OnEnable()
    {
        restartButton.onClick.AddListener(Restart);
    }

    private void OnDisable()
    {
        restartButton.onClick.RemoveListener(Restart);
    }

    public void Show()
    {
        gameObject.SetActive(true);
        Time.timeScale = 0f;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void Restart()
    {
        Time.timeScale = 1f;
        GameSession.StartNewRun();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}