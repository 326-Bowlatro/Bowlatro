using System;
using UnityEngine;

public class ToShopButton : MonoBehaviour
{
    [SerializeField]
    private CameraScript cam;

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
        //GameManager.ShopState.EnterState();
        Disable();
    }
}
