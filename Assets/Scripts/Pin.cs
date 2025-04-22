using System;

using UnityEngine;

public class Pin : MonoBehaviour
{
    public static event Action<int, int> OnPinKnockedOver;
    
    [SerializeField] private int flatScore;
    [SerializeField] private int multScore;

    private Rigidbody rb;
    private Vector3 initialPosition;
    private Vector3 initialRotation;
    
    private bool knockedOver;
    
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        initialPosition = transform.position;
        initialRotation = transform.eulerAngles;
        knockedOver = false;
        //subscribe like and comment to bowling ball reset event
        BowlingBall.OnBallReset += BowlingBallOnOnBallReset;
    }

    private void BowlingBallOnOnBallReset()
    {
        ResetPin();
    }

    private void Update()
    {
        if (!knockedOver && (transform.eulerAngles.x > 60 || transform.eulerAngles.z > 60))
        {
            knockedOver = true;
            //increase score by pin predefined amount
            OnPinKnockedOver?.Invoke(flatScore, multScore);
        }
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

    private void OnDestroy() //unsubscribe because can cause problems if not, i.e. break invokes, break objects etc.
    {
        //unsubscribe
        BowlingBall.OnBallReset -= BowlingBallOnOnBallReset;
    }
}
