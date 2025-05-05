using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Manages player inventory (their "deck").
/// </summary>
public class InventoryManager : MonoBehaviour
{
    public List<PinLayoutCardSO> OwnedItems { get; } = new();

    // Constant array of all possible pin layout cards that exist in the game.
    [SerializeField]
    private List<PinLayoutCardSO> defaultItems;

    public void Start()
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

        // Refresh UI
        GameUI.Instance.Refresh();
    }
}
