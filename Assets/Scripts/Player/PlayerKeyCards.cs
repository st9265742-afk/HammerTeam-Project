using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerKeyCards : MonoBehaviour
{
    private readonly HashSet<KeyCardType> _cards = new HashSet<KeyCardType>();

    public event Action Changed;

    public bool Has(KeyCardType type) => _cards.Contains(type);

    public void Add(KeyCardType type)
    {
        if (_cards.Add(type))
        {
            Changed?.Invoke();
        }
    }
}