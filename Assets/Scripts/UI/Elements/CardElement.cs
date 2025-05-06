using System;
using UnityEngine;
using UnityEngine.UIElements;

[UxmlElement]
public partial class CardElement : VisualElement
{
    /// <summary>
    /// Gets/sets card visibility.
    /// </summary>
    public bool IsHidden
    {
        get => this.Q<Button>("_Button").style.visibility == Visibility.Hidden;
        set
        {
            this.Q<Button>("_Button").style.visibility = value
                ? Visibility.Hidden
                : Visibility.Visible;

            tooltipManipulator.IsHidden = value;
        }
    }

    public event Action OnClick;

    private readonly ToolTipManipulator tooltipManipulator;

    public CardElement()
    {
        Resources.Load<VisualTreeAsset>("Elements/CardElement").CloneTree(this);
        this.Q<Button>("_Button").clicked += () => OnClick?.Invoke();

        // Add tooltip manipulator
        tooltipManipulator = new ToolTipManipulator();
        this.AddManipulator(tooltipManipulator);
    }

    public void SetCard(IInventoryCard card)
    {
        IsHidden = card == null;
        if (!IsHidden)
        {
            SetCard(card.Name, card.Description, card.Sprite);
        }
    }

    public void SetCard(string name, string description, Sprite sprite)
    {
        this.Q<Label>("_Text").text = name;
        tooltipManipulator.Title = name;
        tooltipManipulator.Description = description;
    }
}
