using UnityEngine;

public class CameraScript : MonoBehaviour
{
    [SerializeField] private Transform pinLookTarget;
    [SerializeField] private Transform ballLookTarget;
    [SerializeField] private Transform shopLookTarget;
    [SerializeField] private Transform resultsLookTarget;
    [SerializeField] private Vector3 camBallOffset;
    [SerializeField] private Vector3 camPinOffset;
    [SerializeField] private Vector3 camShopOffset;
    [SerializeField] private Vector3 camResultsOffset;
    [SerializeField] private Vector3 ballLookAtOffset;
    [SerializeField] private Vector3 pinLookAtOffset;
    [SerializeField] private Vector3 shopLookAtOffset;
    [SerializeField] private Vector3 resultsLookAtOffset;

    private bool isLookingAtPins = false;
    private bool isLookingAtShop = false;
    private bool isLookingAtResults = false;

    void Update()
    {
        if (isLookingAtPins)
        {
            transform.position = pinLookTarget.position + camPinOffset;
            transform.LookAt(pinLookTarget.position + pinLookAtOffset);
        }
        else if (isLookingAtShop)
        {
            transform.position = shopLookTarget.position + camShopOffset;
            transform.LookAt(shopLookTarget.position + shopLookAtOffset);
        }
        else if (isLookingAtResults)
        {
            transform.position = resultsLookTarget.position + camResultsOffset;
            transform.LookAt(resultsLookTarget.position + resultsLookAtOffset);
        }
        else
        {
            transform.position = ballLookTarget.position + camBallOffset;
            transform.LookAt(ballLookTarget.position + ballLookAtOffset);
        }
    }

    /// <summary>
    /// Switches camera mode to show the pins.
    /// </summary>
    public void BeginLookAtPins()
    {
        isLookingAtPins = true;
    }

    /// <summary>
    /// Switches camera mode to follow the ball.
    /// </summary>
    public void EndLookAtPins()
    {
        isLookingAtPins = false;
    }
    
    public void BeginLookAtShop()
    {
        isLookingAtShop = true;
    }

    public void EndLookAtShop()
    {
        isLookingAtShop = false;
    }

    public void BeginLookAtResults()
    {
        isLookingAtResults = true;
    }

    public void EndLookAtResults()
    {
        isLookingAtResults = false;
    }
    
    public void OnEndTurn()
    {
        EndLookAtPins();
    }
}
