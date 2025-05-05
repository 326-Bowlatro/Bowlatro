using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class BoosterCardManager : MonoBehaviour
{
    public static BoosterCardManager Instance { get; private set; }
    
    [SerializeField] private List<BoosterCardSO> boosterCardSOs;

    [SerializeField] private List<GameObject> boosterCards;

    [SerializeField] int cardGenerationLimit = 3;

    public int chooseCardLimit = 2;
    public int chosenCardAmount;

    void Awake()
    {
        Instance = this;
    }

    /// <summary>
    /// Will generate cards up to cardGenerationLimit for the player to select for the next round
    /// </summary>
    public void StartSelection()
    {
        //randomly generates 3 card to choose from
        for (int i = 0; i < cardGenerationLimit; i++)
        {
            //creates a random int first in valid range
            int curr = Random.Range(0, boosterCardSOs.Count);
            //instantiate card as a child of current object transform
            GameObject currentCard = Instantiate(boosterCardSOs[curr].prefab, transform);
            //add SO to list of current layout of pin cards
            boosterCards.Add(currentCard);
        }
    }

    public void EndSelection()
    {
        foreach (var boosterCard in boosterCards)
        {
            Destroy(boosterCard);
        }
        boosterCards.Clear();
    }
}
