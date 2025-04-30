using System;
using UnityEngine;

public class ResultsManager : MonoBehaviour
{
    public static ResultsManager Instance { get; private set; }

    [SerializeField] private CameraScript mainCam;
    [SerializeField] private ToShopButton toShopButton;

    public int cashToBeEarned = 0;
    
    private void Start()
    {
        Instance = this;
    }

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
