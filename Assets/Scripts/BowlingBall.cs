using UnityEngine;

public class BowlingBall : MonoBehaviour
{
    //create event for when ball resets
    public delegate void ballReset();

    public static event ballReset OnBallReset;
    
    [SerializeField] private Camera mainCamera;
    
    public float LaunchForce = 1000f;
    public float SteeringForce = 10f;
    public float ResetDelay = 2f;
    public float AutoResetTime = 5f;
    public int MaxThrows = 5;

    private Rigidbody rb;
    private Vector3 startPos;
    private Quaternion startRot;
    private int throwsUsed = 0;
    private bool hasLaunched = false;
    private float launchTime = 0f;
    private float ballLaunchedTime = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        startPos = transform.position;
        startRot = transform.rotation;
    }

    void Update()
    {
        if (throwsUsed >= MaxThrows)
            return;

        
        if (hasLaunched)
        {
            ballLaunchedTime += Time.deltaTime;
            // allow steering
            float steer = 0f;
            if (Input.GetKey(KeyCode.A))
                steer = -1f;
            if (Input.GetKey(KeyCode.D))
                steer = 1f;
            rb.AddForce(Vector3.left * (steer * SteeringForce));
        }
        
        // Launch with W key
        if (!hasLaunched && Input.GetKeyDown(KeyCode.W))
        {
            LaunchBall();
        }

        // Launch with mouse click (on ball)
        // if (!hasLaunched && Input.GetMouseButtonDown(0))
        // {
        //     Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        //     if (Physics.Raycast(ray, out RaycastHit hit) && hit.collider.gameObject == gameObject)
        //     {
        //         LaunchBall();
        //     }
        // }


        // Auto-reset if the ball slows down too much
        if (hasLaunched && ballLaunchedTime >= 1f && rb.linearVelocity.magnitude < 0.05f)
        {
            hasLaunched = false;
            ResetBall();
        }

        // Auto-reset if too much time passes
        if (hasLaunched && ballLaunchedTime >= AutoResetTime)
        {
            hasLaunched = false;
            ResetBall();
        }

        // Reset if ball goes too far off the lane
        if (hasLaunched && Vector3.Distance(transform.position, startPos) > 50f)
        {
            hasLaunched = false;
            Invoke(nameof(ResetBall), ResetDelay); // ???
        }
    }

    private void LaunchBall()
    {
        rb.AddForce(-transform.forward * LaunchForce); // assumes forward is negative Z
        hasLaunched = true;
        throwsUsed++;
    }

    private void ResetBall()
    {
        
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        transform.position = startPos;
        transform.rotation = startRot;

        hasLaunched = false;
        ballLaunchedTime = 0;
        
        //when ball resets, send event signal out
        OnBallReset?.Invoke();
    }
}
