using UnityEngine;

[RequireComponent(typeof(Door))]
public class KeyCardLock : MonoBehaviour, IInteractable
{
    [SerializeField] private KeyCardType requiredCard;

    private Door _door;
    private PlayerKeyCards _keyCards;

    public string InteractionPrompt => "Open door";
    public bool CanInteract => _door != null && !_door.IsOpen && _keyCards != null && _keyCards.Has(requiredCard);

    private void Awake()
    {
        _door = GetComponent<Door>();
        _keyCards = FindAnyObjectByType<PlayerKeyCards>();
    }

    public void Interact(GameObject interactor)
    {
        if (CanInteract)
        {
            _door.Open();
        }
    }
}