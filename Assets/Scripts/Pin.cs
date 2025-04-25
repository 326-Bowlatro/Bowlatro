using UnityEngine;

public class Pin : MonoBehaviour
{

    [SerializeField] private int flatScore;
    [SerializeField] private int multScore;

    public bool IsKnockedOver { get; private set; }

    public int FlatScore;
    public int MultScore;


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
        if (!IsKnockedOver && Vector3.Dot(transform.up, Vector3.up) < 0.8f)
        {
            IsKnockedOver = true;
            //increase score by pin predefined amount
            GameManager.Instance.AddPinToScore(this);
        }
    }

    public void OnEndTurn()

    {
        // Destroy because pins can be knocked down between throws, and it interferes with the current strike detection system
        // Only destroy if knocked down
        if (IsKnockedOver)
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

        //set back to active
        gameObject.SetActive(true);
        //set back to not knocked over
        knockedOver = false;

    }
}
