using System;
using UnityEngine;
using UnityEngine.UIElements;

[UxmlElement]
public partial class CardElement : VisualElement
{
    /// <summary>
    /// Reference to the card displayed by this element.
    /// </summary>
    public IInventoryCard Card { get; private set; }

    /// <summary>
    /// Gets/sets card visibility.
    /// </summary>
    public bool IsHidden
    {
        get => this.Q<Button>("_Button").style.visibility == Visibility.Hidden;
        set =>
            this.Q<Button>("_Button").style.visibility = value
                ? Visibility.Hidden
                : Visibility.Visible;
    }

    public event Action OnClick;

    public CardElement()
    {
        Resources.Load<VisualTreeAsset>("Elements/CardElement").CloneTree(this);
    }

    public CardElement(IInventoryCard card)
        : this()
    {
        Card = card;

        this.Q<Label>("_Text").text = card?.Name;
        this.Q<Button>("_Button").clicked += () => OnClick?.Invoke();
    }
}
