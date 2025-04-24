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
        // Are we less than 80% upright?
        if (!knockedOver && Vector3.Dot(transform.up, Vector3.up) < 0.8f)
        {
            knockedOver = true;
            //increase score by pin predefined amount
            GameManager.Instance.AddPinToScore(flatScore, multScore);
        }
    }

    public void OnEndRound()
    {
        ResetPin();
    }

    public void OnEndTurn()
    {
        // Destroy because pins can be knocked down between throws, and it interferes with the current strike detection system
        // Only destroy if knocked down
        if (knockedOver)
        {
            Destroy(gameObject);
        }
        else
        {
            // To prevent pins mid-fall to keep falling and possibly create a strike
            ResetPin();
        }
    }

    private void ResetPin()
    {
        // Reset position/rotation
        transform.position = initialPosition;
        transform.eulerAngles = initialRotation;
        // Reset velocities
        rb.angularVelocity = Vector3.zero;
        rb.linearVelocity = Vector3.zero;
        // Set back to not knocked over
        knockedOver = false;
    }
}
