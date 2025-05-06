using UnityEngine;
using UnityEngine.UIElements;

[UxmlElement]
public partial class ToolTipElement : VisualElement
{
    public const int Width = 200;
    public const int Height = 100;

    public string Title
    {
        set => this.Q<Label>("_Title").text = value;
    }

    public string Description
    {
        set => this.Q<Label>("_Description").text = value;
    }

    public ToolTipElement()
    {
        Resources.Load<VisualTreeAsset>("Elements/ToolTipElement").CloneTree(this);
        style.width = Width;
        style.height = Height;
        pickingMode = PickingMode.Ignore;
    }
}
