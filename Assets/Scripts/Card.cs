using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Card : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] private GameObject visual;
    private float selectedYMult = 25f;
    private Vector3 originalPos, selectedPos;
    private bool selected;
    private void Start()
    {
        selected = false;
    }

    private void Update()
    {
        // if (selected)
        // {
        //     visual.localPosition += transform.up;
        // }
        // else
        // {
        //     visual.localPosition = Vector3.zero;
        // }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // selected = !selected;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        visual.transform.localPosition += transform.up * selectedYMult;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        visual.transform.localPosition = Vector3.zero;
    }
}
