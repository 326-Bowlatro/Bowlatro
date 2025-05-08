using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Manages shop inventory.
/// </summary>
public class ShopManager : MonoBehaviour
{
    public const int RerollCost = 5;

    // All layout cards available in the game.
    [SerializeField]
    private List<PinLayoutCard> allLayoutCards = new();

    // All booster cards available in the game.
    [SerializeField]
    private List<BoosterCard> allBoosterCards = new();

    // All tickets available in the game.
    [SerializeField]
    private List<Ticket> allTickets = new();

    /// <summary>
    /// Current set of tickets available in the shop.
    /// </summary>
    public List<Ticket> CurrentTickets { get; private set; } = new();

    /// <summary>
    /// Current layout card pack available in the shop. Can be null if the pack has been claimed.
    /// </summary>
    public LayoutCardPack CurrentPack { get; private set; }

    /// <summary>
    /// Clears the existing shop inventory and reinits with a new random selection of items.
    /// </summary>
    public void ResetInventory()
    {
        // Choose layout or booster pack at random.
        CurrentPack = CreatePack(Random.Range(0, 2) == 0);

        // Randomly select 3 unowned tickets to put in the shop.
        CurrentTickets.Clear();
        CurrentTickets.AddRange(
            allTickets
                .Where(ticket => !ticket.IsOwned())
                .OrderBy(x => Random.Range(0, int.MaxValue))
                .Take(3)
        );

        // Refresh UI
        GameUI.Instance.Refresh();
    }

    /// <summary>
    /// Claims a ticket, removing the ticket from shop inventory.
    /// </summary>
    public void ClaimTicket(Ticket ticket)
    {
        // Remove ticket from shop.
        CurrentTickets.Remove(ticket);
        Debug.Log($"Claimed ticket \"{ticket.name}\"");

        // Deduct cost of pack.
        GameManager.Instance.DeductCash(ticket.Cost);

        // Add card to player's inventory.
        GameManager.Instance.InventoryManager.AddItem(ticket);

        // Refresh UI
        GameUI.Instance.Refresh();
    }

    /// <summary>
    /// Claims a card from the current pack, removing the pack from inventory.
    /// </summary>
    public void ClaimPackCard(IInventoryCard card)
    {
        var pack = CurrentPack;

        // Remove pack from shop.
        CurrentPack = null;
        Debug.Log($"Claimed card \"{card.Name}\" from pack \"{pack.PackName}\"");

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
    private LayoutCardPack CreatePack(bool isLayoutPack)
    {
        // TODO: Generate randomly.
        var packCost = 4;

        // Generate a random name for the pack.
        var packName = $"Pin Pack {Random.Range(1, 1000)}";

        // Pick 3 random cards to put in the pack.
        var packCards = (isLayoutPack ? allLayoutCards.Cast<IInventoryCard>() : allBoosterCards)
            .Cast<IInventoryCard>()
            .OrderBy(x => Random.Range(0, int.MaxValue))
            .Take(3);

        return new LayoutCardPack
        {
            PackName = packName,
            PackCost = packCost,
            PackCards = packCards.ToList(),
            IsLayoutPack = isLayoutPack,
        };
    }
}

public record LayoutCardPack
{
    /// <summary>
    /// Is this pack a layout or a booster pack?
    /// </summary>
    public bool IsLayoutPack { get; set; }

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
    public List<IInventoryCard> PackCards { get; set; }
}
