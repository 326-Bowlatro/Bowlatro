using UnityEngine;

/// <summary>
/// Represents a persistent ticket with some effect.
/// </summary>
public abstract class Ticket : ScriptableObject
{
    [Header("Base")]
    public Sprite Sprite;
    public string Name;

    [TextArea(4, 4)]
    public string Description;

    public virtual void Activate() { }
}
