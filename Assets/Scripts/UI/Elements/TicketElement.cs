using System;
using UnityEngine;
using UnityEngine.UIElements;

[UxmlElement]
public partial class TicketElement : VisualElement
{
    /// <summary>
    /// Reference to the ticket displayed by this element.
    /// </summary>
    public Ticket Ticket { get; private set; }

    public event Action OnClick;

    private readonly ToolTipManipulator tooltipManipulator;

    public TicketElement()
    {
        Resources.Load<VisualTreeAsset>("Elements/TicketElement").CloneTree(this);

        // Add tooltip manipulator
        tooltipManipulator = new ToolTipManipulator();
        this.AddManipulator(tooltipManipulator);
    }

    public TicketElement(Ticket ticket)
        : this()
    {
        Ticket = ticket;

        this.Q<Label>("_Text").text = Ticket.Name;
        this.Q<Button>("_Button").clicked += () => OnClick?.Invoke();
        tooltipManipulator.Title = ticket.Name;
        tooltipManipulator.Description = ticket.Description;
    }
}
