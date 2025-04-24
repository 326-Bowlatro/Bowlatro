using System;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    public static CardManager Instance { get; private set; }
    
    [SerializeField] private List<PinLayoutCardUI> cards;

    void Awake()
    {
        Instance = this;
    }

    public void DisableCards()
    {
        foreach (var card in cards)
        {
            card.gameObject.SetActive(false);
        }
    }

    public void EnableCards()
    {
        foreach (var card in cards)
        {
            card.gameObject.SetActive(true);
        }
    }
}
