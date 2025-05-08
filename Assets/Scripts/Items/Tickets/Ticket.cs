using UnityEngine;

/// <summary>
/// Represents a persistent ticket with some effect.
/// </summary>
public abstract class Ticket : ScriptableObject
{
    [Header("Base")]
    public Sprite Sprite;
    public string Name;
    public int Cost;

    [TextArea(4, 4)]
    public string Description;

    public bool IsOwned() => GameManager.Instance.InventoryManager.CurrentTickets.Contains(this);

    public virtual void Activate() { }
}
