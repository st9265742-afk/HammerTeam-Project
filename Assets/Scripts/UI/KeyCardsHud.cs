using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class KeyCardsHud : MonoBehaviour
{
    [SerializeField] private PlayerKeyCards keyCards;
    [SerializeField] private Text label;

    private void OnEnable()
    {
        keyCards.Changed += Refresh;
        Refresh();
    }

    private void OnDisable()
    {
        keyCards.Changed -= Refresh;
    }

    private void Refresh()
    {
        List<string> names = new List<string>();

        foreach (KeyCardType type in Enum.GetValues(typeof(KeyCardType)))
        {
            if (keyCards.Has(type))
            {
                names.Add(type.ToString());
            }
        }

        label.text = names.Count > 0 ? $"Key cards: {string.Join(", ", names)}" : string.Empty;
    }
}