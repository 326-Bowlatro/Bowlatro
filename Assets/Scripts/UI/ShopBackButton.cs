using UnityEngine;

public class ShopBackButton : MonoBehaviour
{
    [SerializeField] private CameraScript cam;
    [SerializeField] private PinCardManager pinCardManager;

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
        pinCardManager.StartSelection();
        GameManager.Instance.EndShop();
        Disable();
    }
}
