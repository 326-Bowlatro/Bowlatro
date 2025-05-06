using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public interface IInventoryCard
{
    public string Name { get; }
    public string Description { get; }
    public Sprite Sprite { get; }
}

/// <summary>
/// Manages player inventory (their "deck").
/// </summary>
public class InventoryManager : MonoBehaviour
{
    public const int MaxLayouts = 3;
    public const int MaxBoosters = 5;
    public const int MaxTickets = 5;

    /// <summary>
    /// List of tickets that the player currently owns.
    /// </summary>
    public List<Ticket> CurrentTickets { get; } = new();

    /// <summary>
    /// List of cards that the player currently owns.
    /// </summary>
    public List<IInventoryCard> CurrentDeck { get; } = new();

    /// <summary>
    /// List of boosters that were drawn in the last call to <see cref="DrawCards"/>
    /// </summary>
    public List<BoosterCard> CurrentHandBoosters { get; private set; } = new();

    /// <summary>
    /// List of boosters that were drawn in the last call to <see cref="DrawCards"/>
    /// </summary>
    public List<PinLayoutCard> CurrentHandLayouts { get; private set; } = new();

    [SerializeField]
    private List<Ticket> defaultTickets;

    [SerializeField]
    private List<PinLayoutCard> defaultDeckLayoutCards;

    [SerializeField]
    private List<BoosterCard> defaultDeckBoosterCards;

    void Awake()
    {
        // Start with some initial cards in the inventory.
        defaultTickets.ForEach(AddItem);
        defaultDeckLayoutCards.ForEach(AddItem);
        defaultDeckBoosterCards.ForEach(AddItem);
    }

    /// <summary>
    /// Adds the given card to the player's inventory.
    /// </summary>
    public void AddItem(IInventoryCard card)
    {
        CurrentDeck.Add(card);
    }

    /// <summary>
    /// Adds the given ticket to the player's inventory.
    /// </summary>
    public void AddItem(Ticket ticket)
    {
        CurrentTickets.Add(ticket);
    }

    /// <summary>
    /// Clears the existing hand and reinits with a new random selection of cards.
    /// </summary>
    public void ResetHand()
    {
        // Pick n random layout cards to return.
        CurrentHandLayouts = CurrentDeck
            .OfType<PinLayoutCard>()
            .OrderBy(x => Random.Range(0, int.MaxValue))
            .Take(MaxLayouts)
            .ToList();

        // Pick n random layout cards to return.
        CurrentHandBoosters = CurrentDeck
            .OfType<BoosterCard>()
            .OrderBy(x => Random.Range(0, int.MaxValue))
            .Take(MaxBoosters)
            .ToList();

        Debug.Log(
            $"InventoryManager: {CurrentHandLayouts.Count} layout cards, {CurrentHandBoosters.Count} booster cards drawn from deck."
        );

        // Refresh UI
        GameUI.Instance.Refresh();
    }
}
