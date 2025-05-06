using UnityEngine;
using UnityEngine.UIElements;

public class ToolTipManipulator : Manipulator
{
    private ToolTipElement element = new ToolTipElement();
    private bool hasAddedElement = false;

    public string Title
    {
        set => element.Title = value;
    }

    public string Description
    {
        set => element.Description = value;
    }

    public bool IsHidden { get; set; } = false;

    public ToolTipManipulator()
        : this("Title", "Description") { }

    public ToolTipManipulator(string title, string description)
    {
        Title = title;
        Description = description;
    }

    protected override void RegisterCallbacksOnTarget()
    {
        target.RegisterCallback<MouseEnterEvent>(MouseEnter);
        target.RegisterCallback<MouseLeaveEvent>(MouseLeave);
        target.RegisterCallback<DetachFromPanelEvent>(DetachFromPanel);
    }

    protected override void UnregisterCallbacksFromTarget()
    {
        target.UnregisterCallback<MouseEnterEvent>(MouseEnter);
        target.UnregisterCallback<MouseLeaveEvent>(MouseLeave);
    }

    private void DetachFromPanel(DetachFromPanelEvent e)
    {
        element.RemoveFromHierarchy();
    }

    private void MouseEnter(MouseEnterEvent e)
    {
        var root = GetRootElement(target);

        if (IsHidden)
        {
            return;
        }

        if (!hasAddedElement)
        {
            GetRootElement(target).Add(element);
            hasAddedElement = true;
        }

        // Prevent tooltip from going out of bounds
        var top = target.worldBound.yMax - 20;
        if (top + ToolTipElement.Height > root.worldBound.yMax)
        {
            var offset = Mathf.CeilToInt(top + ToolTipElement.Height - root.worldBound.yMax);
            top -= offset;
        }

        // Update tooltip placement
        element.style.top = top;
        element.style.left = target.worldBound.center.x - (ToolTipElement.Width / 2);
        element.style.position = Position.Absolute;
        element.style.visibility = Visibility.Visible;
    }

    private void MouseLeave(MouseLeaveEvent e)
    {
        element.style.visibility = Visibility.Hidden;
    }

    private VisualElement GetRootElement(VisualElement subject)
    {
        for (
            VisualElement parent = subject.hierarchy.parent;
            parent != null;
            parent = parent.hierarchy.parent
        )
        {
            if (parent.parent == null)
            {
                return parent;
            }
        }

        return null;
    }
}
