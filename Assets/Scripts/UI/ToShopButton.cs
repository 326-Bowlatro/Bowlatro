using System;
using UnityEngine;

public class ToShopButton : MonoBehaviour
{
    [SerializeField] private CameraScript cam;
    [SerializeField] private PinCardManager pinCardManager;
    
    void Start()
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

    //calls function to go to shop
    public void ToShop()
    {
        cam.EndLookAtResults();
        GameManager.Instance.GoToState<GameManager.ShopState>();
        Disable();
    }
}
