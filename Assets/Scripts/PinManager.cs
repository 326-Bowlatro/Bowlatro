using UnityEngine;

public class PinManager : MonoBehaviour
{
    private Rigidbody rb;
    private Vector3 initialPosition;
    private Quaternion initialRotation;
    private bool isKnockedOver = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        initialPosition = transform.position;
        initialRotation = transform.rotation;
    }

    void Update()
    {
        // Check if the pin is knocked over and hasn't been already registered as such
        if (!isKnockedOver && transform.up.y < 0.5)
        {
            isKnockedOver = true;
            gameObject.SetActive(false); // Optionally deactivate the pin
            ScoreManager.AddScore(1);
        }
    }

    public void ResetPin()
    {
        isKnockedOver = false; // Reset the status
        transform.position = initialPosition;
        transform.rotation = initialRotation;
        gameObject.SetActive(true);
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }
}
