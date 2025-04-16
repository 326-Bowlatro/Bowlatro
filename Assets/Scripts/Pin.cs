using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class Pin : MonoBehaviour
{
    public delegate void pinKnockedOver(float flat, float mult);

    public static event pinKnockedOver OnPinKnockedOver;
    
    [SerializeField] private int flatScore;
    [SerializeField] private float multScore;

    private Rigidbody rb;
    private Vector3 initialPosition;
    private Vector3 initialRotation;
    
    private bool knockedOver;
    
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        initialPosition = transform.position;
        initialRotation = transform.eulerAngles;
        Debug.Log(initialRotation);
        knockedOver = false;
        flatScore = 1;
        multScore = 0;
        //subscribe like and comment to bowling ball reset event
        BowlingBall.OnBallReset += BowlingBallOnOnBallReset;
    }

    private void BowlingBallOnOnBallReset()
    {
        resetPin();
    }

    private void Update()
    {
        if (!knockedOver && (transform.eulerAngles.x > 60 || transform.eulerAngles.z > 60))
        {
            knockedOver = true;
            //increase score by certain amount
            Debug.Log("I've fallen!");
            OnPinKnockedOver?.Invoke(flatScore, multScore);
            StartCoroutine(DelayedSetInactive());
        }
        
    }

    private void resetPin()
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

    IEnumerator DelayedSetInactive()
    {
        yield return new WaitForSeconds(2f);
        gameObject.SetActive(false);
    }
}
