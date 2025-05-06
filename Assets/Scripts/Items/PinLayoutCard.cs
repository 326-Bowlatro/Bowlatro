using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu]
public class PinLayoutCard : ScriptableObject, IInventoryCard
{
    public Sprite Image;
    public LayoutType LayoutType;
    public string LayoutName;

    // IInventoryCard implementation
    string IInventoryCard.Name => LayoutName;
    string IInventoryCard.Description => "No effect ):"; // TODO
    Sprite IInventoryCard.Sprite => Image;
}
