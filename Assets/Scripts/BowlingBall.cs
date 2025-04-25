using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class BowlingBall : MonoBehaviour
{
    [Header("Boss Modifiers")]
    public bool isBossRound = false;
    public bool veerEnabled = false;
    public float veerDirection = 1f; // 1 for right, -1 for left
    public float veerStrength = 5f;
    public bool requireBounce = false;

    [Header("Special Balls")]
    public bool isMultiplierBall = false;
    public bool isBonusBall = false;
    
    [SerializeField] private CameraScript mainCamera;
    [SerializeField] private Transform pinsMainPoint;
    [SerializeField] private Collider laneCollider;
    [SerializeField] private float aimAmount = 1f;

    [SerializeField] private float LaunchForce = 1500f;
    [SerializeField] private float ResetDelay = 2f;

    private Rigidbody rb;
    private Vector3 startPosition;
    private Vector3 startRotation;
    private bool hasLaunched = false;
    private bool reachedPins = false;
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
        if (!reachedPins && Math.Abs((transform.position - pinsMainPoint.position).magnitude) < 10f)
        {
            // Have the camera look at the pins. This will be reset
            // automatically when the turn ends.
            mainCamera.BeginLookAtPins();

            // Set a timer to end the player's turn after a delay.
            StartCoroutine(DelayedEndTurn());

            reachedPins = true;
        }

        if (!hasLaunched)
        {
            //keeps ball in bounds with buffer so the ball doesn't go off the lane
            if (Input.GetKey(KeyCode.A) && transform.position.x < laneMax-.1f)
            {
                var pos = transform.position;
                var newX = pos.x + aimAmount*Time.deltaTime;
                transform.position = new Vector3(newX, pos.y, pos.z);
            }

            if (Input.GetKey(KeyCode.D) && transform.position.x > laneMin+.1f)
            {
                var pos = transform.position;
                var newX = pos.x - aimAmount*Time.deltaTime;
                transform.position = new Vector3(newX, pos.y, pos.z);
            }
        }
    }

    /// <summary>
    /// Launches the ball forward from its starting position.
    /// </summary>
    public void LaunchBall()
    {
        // Can only launch once/turn.
        if (hasLaunched)
        {
            return;
        }
        if (isBossRound && veerEnabled)
        {
            // Making the ball veer
            Vector3 combinedForce = (-transform.forward * LaunchForce) + 
                                    (Vector3.right * (veerDirection * veerStrength));
            rb.AddForce(combinedForce);
            
            // Start maintaining the veer direction
            StartCoroutine(VeerBall());
        }
        else
        {
            // Normal launch
            rb.AddForce(-transform.forward * LaunchForce);
        }
        hasLaunched = true;
    }

    public void OnEndTurn()
    {
        ResetBall();
    }

    private void ResetBall()
    {
        //reset velocities
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        //reset position/rotation
        transform.position = startPosition;
        transform.eulerAngles = startRotation;

        //reset launched bool and timer for bowling ball launch
        hasLaunched = false;
        //reset reached pins bool
        reachedPins = false;
        
        isMultiplierBall = false;
        isBonusBall = false;

    }

    IEnumerator DelayedEndTurn()
    {
        yield return new WaitForSeconds(ResetDelay);
        GameManager.Instance.EndTurn();
    }
    
    public IEnumerator VeerBall()
    {
        
        while (!reachedPins)
        {
            rb.AddForce(
                Vector3.right * (veerDirection * veerStrength),
                ForceMode.Force);
            yield return null;
        }

        
    }

    public IEnumerator SlowDownBall()
    {
        while (hasLaunched && !reachedPins)
        {
            rb.linearVelocity *= 0.99f;
            yield return null;
        }
    }
    public void SetBossRound(bool isBoss, bool veerActive)
    {
        isBossRound = isBoss;
        veerEnabled = veerActive;
    }

}
