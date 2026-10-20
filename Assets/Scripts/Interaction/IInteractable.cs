using UnityEngine;

public interface IInteractable
{
    string InteractionPrompt { get; }
    bool CanInteract { get; }

    void Interact(GameObject interactor);
}