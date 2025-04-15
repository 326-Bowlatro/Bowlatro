using UnityEngine;

public class BowlingBall : MonoBehaviour
{
    public float launchForce = 500f;
    public float steeringForce = 10f;
    public float resetDelay = 2f;
    public float autoResetTime = 5f;
    public int maxThrows = 5;

    private Rigidbody rb;
    private Vector3 startPos;
    private Quaternion startRot;
    private int throwsUsed = 0;
    private bool hasLaunched = false;
    private float launchTime = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        startPos = transform.position;
        startRot = transform.rotation;
    }

    void Update()
    {
        if (throwsUsed >= maxThrows) return;

        // Launch with W key
        if (!hasLaunched && Input.GetKeyDown(KeyCode.W))
        {
            LaunchBall();
        }

        // Launch with mouse click (on ball)
        if (!hasLaunched && Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit) && hit.collider.gameObject == gameObject)
            {
                LaunchBall();
            }
        }

        // Allow steering while ball is moving
        if (hasLaunched)
        {
            float steer = 0f;
            if (Input.GetKey(KeyCode.A)) steer = -1f;
            if (Input.GetKey(KeyCode.D)) steer = 1f;
            rb.AddForce(Vector3.right * steer * steeringForce);
        }

        // Auto-reset if the ball slows down too much
        if (hasLaunched && Time.time - launchTime >= 1f && rb.linearVelocity.magnitude < 0.05f)
        {
            hasLaunched = false;
            ResetBall();
        }

        // Auto-reset if too much time passes
        if (hasLaunched && Time.time - launchTime >= autoResetTime)
        {
            hasLaunched = false;
            ResetBall();
        }

        // Reset if ball goes too far off the lane
        if (Vector3.Distance(transform.position, startPos) > 50f)
        {
            hasLaunched = false;
            Invoke(nameof(ResetBall), resetDelay);
        }
    }

    void LaunchBall()
    {
        rb.AddForce(-transform.forward * launchForce); // assumes forward is negative Z
        hasLaunched = true;
        launchTime = Time.time;
        throwsUsed++;
    }

    void ResetBall()
    {
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        transform.position = startPos;
        transform.rotation = startRot;
        rb.Sleep();

        hasLaunched = false;
    }
}
