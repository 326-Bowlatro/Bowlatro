using UnityEngine;

public class Pin : MonoBehaviour
{
    [SerializeField] private int flatScore;
    [SerializeField] private int multScore;

    private Rigidbody rb;
    private Vector3 initialPosition;
    private Vector3 initialRotation;

    private bool knockedOver;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        initialPosition = transform.position;
        initialRotation = transform.eulerAngles;
        knockedOver = false;
    }

    void Update()
    {
        if (!knockedOver && (transform.eulerAngles.x > 60 || transform.eulerAngles.z > 60))
        {
            knockedOver = true;
            
            var bowlingBall = FindObjectOfType<BowlingBall>();
            int finalFlat = flatScore;
            int finalMult = multScore;
    
            if ( bowlingBall != null)
            {
                if (bowlingBall.isMultiplierBall)
                {
                    finalMult *= 2;
                }
                if (bowlingBall.isBonusBall)
                {
                    finalFlat += 100;
                }
            }
            
            //increase score by pin predefined amount
            GameManager.Instance.AddPinToScore(finalFlat, finalMult);
        }
    }

    public void OnEndTurn()
    {
        ResetPin();
    }

    private void ResetPin()
    {
        //reset position/rotation
        transform.position = initialPosition;
        transform.eulerAngles = initialRotation;
        //reset velocities
        rb.angularVelocity = Vector3.zero;
        rb.linearVelocity = Vector3.zero;
        //set back to active
        gameObject.SetActive(true);
        //set back to not knocked over
        knockedOver = false;
    }
}
