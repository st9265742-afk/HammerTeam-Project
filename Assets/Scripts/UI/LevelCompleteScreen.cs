using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelCompleteScreen : MonoBehaviour
{
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private Text killsText;
    [SerializeField] private Text timeText;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button nextButton;

    private string _nextScene;
    private LevelStats _stats;

    private void Awake()
    {
        _stats = FindAnyObjectByType<LevelStats>();

        if (_stats == null)
        {
            Debug.LogError("LevelStats not found in scene", this);
        }
    }

    private void OnEnable()
    {
        restartButton.onClick.AddListener(Restart);
        nextButton.onClick.AddListener(Next);
    }

    private void OnDisable()
    {
        restartButton.onClick.RemoveListener(Restart);
        nextButton.onClick.RemoveListener(Next);
    }

    public void Show(string nextScene)
    {
        _nextScene = nextScene;

        killsText.text = $"Kills: {_stats.Kills}";
        timeText.text = $"Time: {TimeSpan.FromSeconds(_stats.Elapsed):mm\\:ss}";
        nextButton.gameObject.SetActive(!string.IsNullOrEmpty(nextScene));

        gameObject.SetActive(true);
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void Next()
    {
        Time.timeScale = 1f;
        playerHealth.SaveToSession();
        SceneManager.LoadScene(_nextScene);
    }

    private void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}