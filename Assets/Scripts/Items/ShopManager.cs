using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Manages shop inventory.
/// </summary>
public class ShopManager : MonoBehaviour
{
    public const int RerollCost = 5;

    // Constant array of all possible pin layout cards that exist in the game.
    [SerializeField]
    private List<PinLayoutCard> allPinLayoutCards;

    /// <summary>
    /// Current layout card pack available in the shop. Can be null if the pack has been claimed.
    /// </summary>
    public LayoutCardPack CurrentPack { get; private set; }

    /// <summary>
    /// Clears the existing shop inventory and reinits with a new random selection of items.
    /// </summary>
    public void ResetInventory()
    {
        CurrentPack = CreatePack();

        // Refresh UI
        GameUI.Instance.Refresh();
    }

    /// <summary>
    /// Claims a card from the current pack, removing the pack from inventory.
    /// </summary>
    public void ClaimPackCard(PinLayoutCard card)
    {
        var pack = CurrentPack;

        // Remove pack from shop.
        CurrentPack = null;
        Debug.Log($"Claimed card \"{card.name}\" from pack \"{pack.PackName}\"");

        // Deduct cost of pack.
        GameManager.Instance.DeductCash(pack.PackCost);

        // Add card to player's inventory.
        GameManager.Instance.InventoryManager.AddItem(card);

        // Refresh UI
        GameUI.Instance.Refresh();
    }

    /// <summary>
    /// Creates a new layout card pack with a random selection of cards.
    /// </summary>
    private LayoutCardPack CreatePack()
    {
        // TODO: Generate randomly.
        var packCost = 4;

        // Generate a random name for the pack.
        var packName = $"Pin Pack {Random.Range(1, 1000)}";

        // Pick 3 random cards to put in the pack.
        var packCards = allPinLayoutCards.OrderBy(x => Random.Range(0, int.MaxValue)).Take(3);

        return new LayoutCardPack
        {
            PackName = packName,
            PackCost = packCost,
            PackCards = packCards.ToList(),
        };
    }
}

public record LayoutCardPack
{
    /// <summary>
    /// Display name of the pack.
    /// </summary>
    public string PackName { get; set; }

    /// <summary>
    /// Cost of the pack when purchased from the shop.
    /// </summary>
    public int PackCost { get; set; }

    /// <summary>
    /// Collection of layout cards in this pack.
    /// </summary>
    public List<PinLayoutCard> PackCards { get; set; }
}
