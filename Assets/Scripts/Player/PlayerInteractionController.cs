using UnityEngine;

public class PlayerInteractionController : MonoBehaviour
{
    [Header("Interaction")]
    [SerializeField] private Camera playerCamera;
    [SerializeField] private float interactionRange = 3f;
    [SerializeField] private InteractionPrompt interactionPrompt;

    private InputManager _inputManager;
    private IInteractable _currentInteractable;

    private void Awake()
    {
        _inputManager = GetComponent<InputManager>();
    }

    private void Update()
    {
        UpdateCurrentInteractable();

        if (_inputManager.Interact && _currentInteractable != null)
        {
            _currentInteractable.Interact();
        }
    }

    private void UpdateCurrentInteractable()
    {
        IInteractable interactable = GetInteractable();

        if (interactable == _currentInteractable)
        {
            return;
        }

        _currentInteractable = interactable;

        if (_currentInteractable != null)
        {
            interactionPrompt.Show(_currentInteractable.InteractionPrompt);
        }
        else
        {
            interactionPrompt.Hide();
        }
    }

    private IInteractable GetInteractable()
    {
        Ray ray = new Ray(
            playerCamera.transform.position,
            playerCamera.transform.forward
        );

        if (!Physics.Raycast(ray, out RaycastHit hit, interactionRange))
        {
            return null;
        }

        return hit.collider.GetComponentInParent<IInteractable>();
    }
}