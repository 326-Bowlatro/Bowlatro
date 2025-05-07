using System;
using System.Collections;
using UnityEngine;

public class BowlingBall : MonoBehaviour
{
    public bool HasLaunched { get; private set; } = false;

    [Header("Boss Modifiers")]
    public bool isBossRound = false;
    public bool veerEnabled = false;
    public float veerDirection ; // 1 for right, -1 for left
    public float veerStrength = 5f;
    public float slowDownFactor = 0.78f;
    public bool slowDownEnabled = false;

    [Header("Special Balls")]
    public bool isMultiplierBall = false;
    public bool isBonusBall = false;
    
    [SerializeField] private CameraScript mainCamera;
    [SerializeField] private Transform pinsMainPoint;
    [SerializeField] private Collider laneCollider;
    [SerializeField] private float aimAmount = 1f;
    [SerializeField] private float turnAmount = 15f;
    [SerializeField] private GameObject aimLine;
    
    public float LaunchForce = 800f;
    [SerializeField] private float ResetDelay = 4f;

    private Rigidbody rb;
    private Vector3 startPosition;
    private Vector3 startRotation;
    private Vector3 startScale;
    private float startMass;  
    private bool reachedPins = false;
    private float laneMax, laneMin;
    private float startLaunchForce;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        startPosition = transform.position;
        startRotation = transform.eulerAngles;
        startScale = transform.localScale;
        startMass = rb.mass;
        laneMax = laneCollider.bounds.max.x;
        laneMin = laneCollider.bounds.min.x;
        startLaunchForce = LaunchForce;
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
    }

    /// <summary>
    /// Checks for movement input and moves the ball accordingly.
    /// </summary>
    public void ProcessMovement()
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

        // if (Input.GetKey(KeyCode.LeftArrow) && transform.eulerAngles.y is < 60 or > 300) //turn left, have a maximum angle to turn to
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            //change rotation leftwards
            Vector3 rotation = transform.eulerAngles;
            transform.eulerAngles = new Vector3(rotation.x, rotation.y - (turnAmount * Time.deltaTime), rotation.z);
        }

        // if (Input.GetKey(KeyCode.RightArrow) && transform.eulerAngles.y is < 60 or > 300) //turn right, have a maximum angle to turn to
        if (Input.GetKey(KeyCode.RightArrow))
        {
            //change rotation rightwards
            Vector3 rotation = transform.eulerAngles;
            transform.eulerAngles = new Vector3(rotation.x, rotation.y + (turnAmount * Time.deltaTime), rotation.z);
        }
    }

    /// <summary>
    /// Launches the ball forward from its starting position.
    /// </summary>
    public void LaunchBall()
    {
        // Can only launch once/turn.
        if (HasLaunched)
        {
            return;
        }
        
        aimLine.SetActive(false);
        
        if (isBossRound && veerEnabled)
        {
            // Making the ball veer
            var combinedForce = (-transform.forward * LaunchForce) + 
                                (Vector3.right * (veerDirection * veerStrength));
            rb.AddForce(combinedForce);
            
            // Start maintaining the veer direction
            StartCoroutine(VeerBall());
        }
        else if (isBossRound && slowDownEnabled)
        {
            // Making the ball veer
            var slowDownForce = (-transform.forward * (LaunchForce * slowDownFactor));
            rb.AddForce(slowDownForce);
            
            // Start maintaining the veer direction
            StartCoroutine(SlowDownBall());
        }
        else
        {
            // Normal launch
            rb.AddForce(-transform.forward * LaunchForce);
        }
        
        AudioSource audio = GetComponent<AudioSource>();
        if (audio != null && !audio.isPlaying)
        {
            Debug.Log("🎳 Playing bowling ball roll sound!");
            audio.Play();
        }

        HasLaunched = true;
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
        
        //reset mass/scale
        transform.localScale = startScale;
        rb.mass = startMass;
        
        //reset launch force that accounts for mass
        LaunchForce = startLaunchForce;
            
        //reset aim line
        aimLine.SetActive(true);
        
        //reset launched bool and timer for bowling ball launch
        HasLaunched = false;
        //reset reached pins bool
        reachedPins = false;

    }

    IEnumerator DelayedEndTurn()
    {
        yield return new WaitForSeconds(ResetDelay);
        GameManager.Instance.EndTurn();
    }
    
    private IEnumerator VeerBall()
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
        while (HasLaunched && !reachedPins)
        {
            rb.AddForce(-transform.forward * (slowDownFactor * LaunchForce));
            yield return null;
        }
    }
    
    public void SetBossBlind(bool isBoss)
    {
        isBossRound = isBoss;
    }

    public bool HasBallReachedPins() => reachedPins;
    public Rigidbody GetRigidbody() => rb;

    public void SetVeerEnabled(bool veerActive, float f)
    {
        veerEnabled = veerActive;
        veerDirection = f;
    }

    public void SetSlowDownEnabled(bool slowDown)
    {
        slowDownEnabled = slowDown;
    }
}