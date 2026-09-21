using UnityEngine;
using UnityEngine.UI;

public class InteractionPrompt : MonoBehaviour
{
    [SerializeField] private Text promptText;

    public void Show(string message)
    {
        promptText.text = $"[E] {message}";
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    private void Awake()
    {
        Hide();
    }
}