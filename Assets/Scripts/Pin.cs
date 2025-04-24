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

    public void OnEndTurn()
    {
        ResetPin();
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
}
