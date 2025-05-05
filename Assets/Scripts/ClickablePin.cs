using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ClickablePin : MonoBehaviour, IPointerClickHandler
{
    private Shop shop;

    public void Initialize(Shop shopReference) {
        shop = shopReference;
        gameObject.AddComponent<Outline>().enabled = false;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        shop.SelectPin(gameObject);
    }
}