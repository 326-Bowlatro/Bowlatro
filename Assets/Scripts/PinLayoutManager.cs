using System;
using UnityEngine;

public class PinLayoutManager : MonoBehaviour
{
    private void Start()
    {
        PinLayoutCard.PinLayoutSelected += PinLayoutCardOnPinLayoutSelected;
    }

    private void PinLayoutCardOnPinLayoutSelected(LayoutEnum layoutType)
    {
        //create layout to be what the enum states
    }

    private void OnDestroy()
    {
        PinLayoutCard.PinLayoutSelected -= PinLayoutCardOnPinLayoutSelected;
    }
}
