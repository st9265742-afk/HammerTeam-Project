using System.Collections.Generic;
using UnityEngine;

public class PlayerKeyCards : MonoBehaviour
{
    private readonly HashSet<KeyCardType> _cards = new HashSet<KeyCardType>();

    public bool Has(KeyCardType type) => _cards.Contains(type);

    public void Add(KeyCardType type)
    {
        _cards.Add(type);
    }
}