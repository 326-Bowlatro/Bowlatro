using UnityEngine;

/// <summary>
/// Represents a consumable booster with some effect.
/// </summary>
public abstract class BoosterCard : ScriptableObject, IInventoryCard
{
    [Header("Base")]
    public Sprite Sprite;
    public string Name;

    [TextArea(4, 4)]
    public string Description;

    public virtual void Activate() { }

    // IInventoryCard implementation
    string IInventoryCard.Name => Name;
    string IInventoryCard.Description => Description;
    Sprite IInventoryCard.Sprite => Sprite;
}
