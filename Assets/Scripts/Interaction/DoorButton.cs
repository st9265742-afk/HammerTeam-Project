using UnityEngine;

public class DoorButton : MonoBehaviour, IInteractable
{
    [SerializeField] private Door door;

    public string InteractionPrompt => door.IsOpen ? "Close door" : "Open door";
    public bool CanInteract => door != null;

    public void Interact(GameObject interactor)
    {
        if (door.IsOpen)
        {
            door.Close();
        }
        else
        {
            door.Open();
        }
    }
}