using System;
using UnityEngine;

public class ResultsManager : MonoBehaviour
{
    [SerializeField] private CameraScript mainCam;
    [SerializeField] private ToShopButton toShopButton;

    public int cashToBeEarned = 0;

    public void Enable()
    {
        mainCam.BeginLookAtResults();
        toShopButton.Enable();
        Debug.Log(cashToBeEarned);
    }

    public void Disable()
    {
        
    }
}
