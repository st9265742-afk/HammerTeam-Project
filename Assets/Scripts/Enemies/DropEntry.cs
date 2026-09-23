using UnityEngine;

[System.Serializable]
public class DropEntry
{
    public GameObject prefab;
    [Range(0f, 1f)]
    public float chance;
}