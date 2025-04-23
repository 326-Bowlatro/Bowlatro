using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class PinLayoutCard : MonoBehaviour, IPointerClickHandler
{
    public static event Action<LayoutEnum> PinLayoutSelected;
    
    [SerializeField] private PinLayoutCardSO pinCardSO;
    [SerializeField] private TextMeshProUGUI cardName;

    private void Start()
    {
        cardName.text = pinCardSO.layoutName;
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        PinLayoutSelected?.Invoke(pinCardSO.layoutType);
        Debug.Log("Card Cliced!!!");
    }
}
