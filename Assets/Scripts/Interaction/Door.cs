using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    [SerializeField] private KeyCardType requiredCard;
    [SerializeField] private Transform panel;
    [SerializeField] private Vector3 openOffset = new Vector3(0f, 3f, 0f);
    [SerializeField] private float openSpeed = 2f;

    private Vector3 _openPosition;
    private bool _isOpen;

    public string InteractionPrompt => $"Open door ({requiredCard} key card)";
    public bool CanInteract => !_isOpen;

    private void Awake()
    {
        _openPosition = panel.localPosition + openOffset;
    }

    private void Update()
    {
        if (!_isOpen)
        {
            return;
        }

        panel.localPosition = Vector3.MoveTowards(
            panel.localPosition,
            _openPosition,
            openSpeed * Time.deltaTime
        );
    }

    public void Interact(GameObject interactor)
    {
        if (_isOpen)
        {
            return;
        }

        PlayerKeyCards keyCards = interactor.GetComponentInParent<PlayerKeyCards>();

        if (keyCards == null || !keyCards.Has(requiredCard))
        {
            Debug.Log($"Need {requiredCard} key card");
            return;
        }

        _isOpen = true;
    }
}