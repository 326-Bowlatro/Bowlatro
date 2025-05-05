using System.Collections.Generic;
using UnityEngine;

public class PinCardManager : MonoBehaviour
{
    public static PinCardManager Instance { get; private set; }
    
    [SerializeField] private List<PinLayoutCardSO> pinLayoutCardSOs;

    [SerializeField] private List<GameObject> pinCards;

    [SerializeField] int cardGenerationLimit = 3;

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
            int curr = Random.Range(0, pinLayoutCardSOs.Count);
            //instantiate card as a child of current object transform
            GameObject currentCard = Instantiate(pinLayoutCardSOs[curr].prefab, transform);
            //add SO to list of current layout of pin cards
            pinCards.Add(currentCard);
        }
    }

    public void EndSelection()
    {
        foreach (var pinCard in pinCards)
        {
            Destroy(pinCard);
        }
        pinCards.Clear();
    }
}
