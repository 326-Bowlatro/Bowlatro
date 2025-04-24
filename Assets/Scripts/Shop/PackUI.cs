using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using System;

public class PackUI : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Image iconImage;

    public TextMeshProUGUI nameText;
    public TextMeshProUGUI priceText;
    
    public GameObject highlight;
    public GameObject soldOverlay;

    // Gets a reference to each pack
    public ItemPack Item { get; private set; } 

    // Setup for the interactions
    private Action<PackUI> onClick;
    private Action<string> onHover;
    private Action onExitHover;

    // Setting the UI visuals of the packs
    public void SetItem(ItemPack item)
    {
        Item = item;
        iconImage.sprite = item.icon;
        nameText.text = item.packName;
        priceText.text = $"${item.price}";
        soldOverlay.SetActive(item.isBought); 
        highlight.SetActive(false);           
    }
    
    public void OnClick(Action<PackUI> callback) => onClick = callback;
    public void OnHover(Action<string> callback) => onHover = callback;
    public void OnExitHover(Action callback) => onExitHover = callback;

    // When this pack is clicked
    public void OnPointerClick(PointerEventData eventData) => onClick?.Invoke(this);

    // When pointer hovers over this pack
    public void OnPointerEnter(PointerEventData eventData) => onHover?.Invoke(Item.description);

    // When pointer leaves this pack
    public void OnPointerExit(PointerEventData eventData) => onExitHover?.Invoke();

    public void Select() => highlight.SetActive(true); 
    public void Deselect() => highlight.SetActive(false);

    // When pack is bought
    public void MarkAsBought()
    {
        soldOverlay.SetActive(true);
        highlight.SetActive(false);
    }
}
