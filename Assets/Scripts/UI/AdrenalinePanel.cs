using UnityEngine;

public class AdrenalinePanel : MonoBehaviour
{
    [SerializeField] private GameObject panel;

    private void Awake()
    {
        panel.SetActive(false);
    }

    public void SetVisible(bool isVisible)
    {
        panel.SetActive(isVisible);
    }
}