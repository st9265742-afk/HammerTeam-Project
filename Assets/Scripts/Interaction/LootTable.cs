using UnityEngine;

[System.Serializable]
public struct LootEntry
{
    public GameObject pickupPrefab;
    [Min(0f)] public float weight;
}

[CreateAssetMenu(menuName = "Game/Loot Table", fileName = "LootTable")]
public class LootTable : ScriptableObject
{
    [SerializeField] private LootEntry[] entries;
    [SerializeField, Min(0f)] private float nothingWeight = 60f;

    public void TrySpawnLoot(Vector3 position)
    {
        float total = nothingWeight;

        foreach (LootEntry entry in entries)
        {
            total += entry.weight;
        }

        float roll = Random.Range(0f, total);

        if (roll < nothingWeight)
        {
            return;
        }

        roll -= nothingWeight;

        foreach (LootEntry entry in entries)
        {
            if (roll < entry.weight)
            {
                Instantiate(entry.pickupPrefab, position, Quaternion.identity);
                return;
            }

            roll -= entry.weight;
        }
    }
}