using System;
using Unity.VisualScripting;
using UnityEngine;

public class ShopBackButtonScript : MonoBehaviour
{
    [SerializeField] private CameraScript cam;
    [SerializeField] private CardManager cardManager;

    private void Start()
    {
        Disable();
    }

    public void Enable()
    {
        gameObject.SetActive(true);   
    }
    
    public void Disable()
    {
        gameObject.SetActive(false);
    }

    //calls functions to leave shop
    public void LeaveShop()
    {
        cam.EndLookAtShop();
        cardManager.EnableCards();
        Disable();
    }
}
