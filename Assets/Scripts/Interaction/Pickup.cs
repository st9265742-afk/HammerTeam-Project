using UnityEngine;

public abstract class Pickup : MonoBehaviour, IInteractable
{
    public abstract string InteractionPrompt { get; }

    public void Interact(GameObject interactor)
    {
        TryPickUpFrom(interactor);
    }

    private void OnTriggerEnter(Collider other)
    {
        TryPickUpFrom(other.gameObject);
    }

    private void TryPickUpFrom(GameObject target)
    {
        PlayerInventory inventory = target.GetComponentInParent<PlayerInventory>();

        if (inventory == null)
        {
            return;
        }

        if (TryPickUp(inventory))
        {
            Destroy(gameObject);
        }
    }

    protected abstract bool TryPickUp(PlayerInventory inventory);
}