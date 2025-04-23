using System;
using System.Collections;
using UnityEngine;

public class BowlingBall : MonoBehaviour
{
    //create event for when ball resets
    public static event Action OnBallReset;

    public static event Action OnBallDie;
    public static event Action OnBallReachedPins;

    [SerializeField] private Collider laneCollider;
    [SerializeField] private float aimAmount = 1f;

    [SerializeField] private float LaunchForce = 1500f;
    [SerializeField] private float ResetDelay = 2f;
    [SerializeField] private float AutoResetTime = 5f;

    private Rigidbody rb;
    private Vector3 startPosition;
    private Vector3 startRotation;
    private bool hasLaunched = false;
    private float ballLaunchedTime = 0f;
    private float laneMax, laneMin;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        startPosition = transform.position;
        startRotation = transform.eulerAngles;
        laneMax = laneCollider.bounds.max.x;
        laneMin = laneCollider.bounds.min.x;
    }

    void Update()
    {
        if (hasLaunched)
        {
            ballLaunchedTime += Time.deltaTime;
        }
        else
        {
            //keeps ball in bounds with buffer so the ball doesn't go off the lane
            // if (Input.GetKey(KeyCode.A) && transform.position.x < laneMax-.1f)
            if(Input.GetKey(KeyCode.A))
            {
                Vector3 pos = transform.position;
                float newX = pos.x + aimAmount*Time.deltaTime;
                transform.position = new Vector3(newX, pos.y, pos.z);
                rb.angularVelocity = Vector3.zero;
                rb.linearVelocity = Vector3.zero;
            }

            // if (Input.GetKey(KeyCode.D) && transform.position.x > laneMin+.1f)
            if(Input.GetKey(KeyCode.D))
            {
                Vector3 pos = transform.position;
                float newX = pos.x - aimAmount*Time.deltaTime;
                transform.position = new Vector3(newX, pos.y, pos.z);
                rb.angularVelocity = Vector3.zero;
                rb.linearVelocity = Vector3.zero;
            }
        }

        // Launch with W key
        if (!hasLaunched && Input.GetKeyDown(KeyCode.W))
        {
            LaunchBall();
        }

        // resets when velocity is low enough
        if (hasLaunched && ballLaunchedTime >= 1f && rb.linearVelocity.magnitude < 0.05f)
        {
            hasLaunched = false;
            StartCoroutine(DelayedBallReset());
        }

        // reset if auto reset time is reached
        if (hasLaunched && ballLaunchedTime >= AutoResetTime)
        {
            hasLaunched = false;
            Destroy(gameObject);
            // ResetBall();
        }
    }

    private void LaunchBall()
    {
        rb.AddForce(-transform.forward * LaunchForce);
        hasLaunched = true;
    }

    private void ResetBall()
    {
        // //reset velocities
        // rb.linearVelocity = Vector3.zero;
        // rb.angularVelocity = Vector3.zero;
        // //reset position/rotation
        // transform.position = startPosition;
        // transform.eulerAngles = startRotation;
        //
        // //reset launched bool and timer for bowling ball launch
        // hasLaunched = false;
        // ballLaunchedTime = 0;

        //when ball resets, send event signal out
        OnBallReset?.Invoke();
    }

    IEnumerator DelayedBallReset()
    {
        yield return new WaitForSeconds(ResetDelay);
        ResetBall();
    }

    private void OnDestroy()
    {
        OnBallDie?.Invoke();
    }
}
