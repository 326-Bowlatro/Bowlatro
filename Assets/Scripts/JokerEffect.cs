using UnityEngine;

public abstract class JokerEffect : ScriptableObject
{
    public string jokerName;
    public Sprite icon;

    public abstract void ApplyEffect(GameObject context);
}
