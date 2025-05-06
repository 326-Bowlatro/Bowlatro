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
        this.Q<Button>("_Button").clicked += () => OnClick?.Invoke();
    }

    public void SetCard(IInventoryCard card)
    {
        Card = card;
        IsHidden = card == null;
        if (!IsHidden)
        {
            SetCard(card.Name, card.Description, card.Sprite);
        }
    }

    public void SetCard(string name, string description, Sprite sprite)
    {
        this.Q<Label>("_Text").text = name;
    }
}
