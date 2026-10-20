using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    [SerializeField] private KeyCardType requiredCard;
    [SerializeField] private Transform panel;
    [SerializeField] private Vector3 openOffset = new Vector3(0f, 3f, 0f);
    [SerializeField] private float openSpeed = 2f;

    private PlayerKeyCards _keyCards;
    private Vector3 _openPosition;
    private bool _isOpen;

    public string InteractionPrompt => "Open door";
    public bool CanInteract => !_isOpen && _keyCards != null && _keyCards.Has(requiredCard);

    private void Awake()
    {
        _openPosition = panel.localPosition + openOffset;
        _keyCards = FindAnyObjectByType<PlayerKeyCards>();
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