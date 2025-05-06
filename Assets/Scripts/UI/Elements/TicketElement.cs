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

    public TicketElement()
    {
        Resources.Load<VisualTreeAsset>("Elements/TicketElement").CloneTree(this);
    }

    public TicketElement(Ticket ticket)
        : this()
    {
        Ticket = ticket;

        this.Q<Label>("_Text").text = Ticket.Name;
        this.Q<Button>("_Button").clicked += () => OnClick?.Invoke();
    }
}
