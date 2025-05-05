using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Manages player inventory (their "deck").
/// </summary>
public class InventoryManager : MonoBehaviour
{
    public const int MaxHandSize = 4;

    /// <summary>
    /// List of cards that the player currently owns.
    /// </summary>
    public List<PinLayoutCardSO> CurrentDeck { get; } = new();

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
        CurrentDeck.Add(card);

        Debug.Log($"Added {card.GetType().Name} {card} to inventory");
    }

    /// <summary>
    /// Clears the existing hand and reinits with a new random selection of cards.
    /// </summary>
    public void ResetHand()
    {
        // Don't try to draw more cards than we have.
        var numCards = Mathf.Min(MaxHandSize, CurrentDeck.Count);

        // Pick n random cards to return.
        CurrentHand = CurrentDeck
            .OrderBy(x => Random.Range(0, int.MaxValue))
            .Take(numCards)
            .ToList();

        // Refresh UI
        GameUI.Instance.Refresh();
    }
}
