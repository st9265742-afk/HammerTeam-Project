using UnityEngine;

[RequireComponent(typeof(Collider))]
public abstract class ItemPickup : MonoBehaviour
{
    private void Awake()
    {
        GetComponent<Collider>().isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerInventory inventory = other.GetComponentInParent<PlayerInventory>();

        if (inventory == null)
        {
            return;
        }

        Debug.Log($"Picked {gameObject.name}");
        Collect(inventory);
        Destroy(gameObject);
    }

    protected abstract void Collect(PlayerInventory inventory);
}