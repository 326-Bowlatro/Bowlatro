using UnityEngine;

public class Pin : MonoBehaviour
{
    public bool IsKnockedOver { get; private set; }

    [SerializeField] private int flatScore;
    [SerializeField] private int multScore;

    private Rigidbody rb;
    private Vector3 initialPosition;
    private Vector3 initialRotation;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        initialPosition = transform.position;
        initialRotation = transform.eulerAngles;
        IsKnockedOver = false;
    }

    void Update()
    {
        // Are we less than 80% upright?
        if (!IsKnockedOver && Vector3.Dot(transform.up, Vector3.up) < 0.8f)
        {
            IsKnockedOver = true;
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
        if (IsKnockedOver)
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
        IsKnockedOver = false;
    }
}
