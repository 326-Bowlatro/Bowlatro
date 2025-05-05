using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Manages player inventory (their "deck").
/// </summary>
public class InventoryManager : MonoBehaviour
{
    /// <summary>
    /// List of cards that the player currently owns.
    /// </summary>
    public List<PinLayoutCardSO> OwnedItems { get; } = new();

    /// <summary>
    /// List of cards that were drawn in the last call to <see cref="DrawCards"/>
    /// </summary>
    public List<PinLayoutCardSO> CurrentHand { get; private set; } = new();

    // Constant array of all possible pin layout cards that exist in the game.
    [SerializeField]
    private List<PinLayoutCardSO> defaultItems;

    void Awake()
    {
        // Start with some initial cards in the inventory.
        defaultItems.ForEach(AddItem);
    }

    /// <summary>
    /// Adds the given card to the player's inventory.
    /// </summary>
    public void AddItem(PinLayoutCardSO card)
    {
        OwnedItems.Add(card);

        Debug.Log($"Added {card.GetType().Name} {card} to inventory");
    }

    /// <summary>
    /// Clears the existing hand and reinits with a new random selection of cards.
    /// </summary>
    public void ResetHand(int numCards)
    {
        // Don't try to draw more cards than we have.
        numCards = Mathf.Min(numCards, OwnedItems.Count);

        // Pick n random cards to return.
        CurrentHand = OwnedItems
            .OrderBy(x => Random.Range(0, int.MaxValue))
            .Take(numCards)
            .ToList();

        // Refresh UI
        GameUI.Instance.Refresh();
    }
}
