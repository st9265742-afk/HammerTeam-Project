using UnityEngine;

public class Pickup : MonoBehaviour, IInteractable
{
    public string InteractionPrompt => "Pick up";

    public void Interact()
    {
        Debug.Log($"Player picked up {gameObject.name}");
        Destroy(gameObject);
    }
}