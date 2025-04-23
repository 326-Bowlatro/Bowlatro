using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class PinLayoutCard : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private PinLayoutCardSO pinCardSO;

    public static event Action<LayoutEnum> PinLayoutSelected; 

    public void OnPointerClick(PointerEventData eventData)
    {
        PinLayoutSelected?.Invoke(pinCardSO.layoutType);
    }
}
