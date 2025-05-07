using UnityEngine;

public class Pin : MonoBehaviour
{
    public bool IsKnockedOver { get; private set; }

    public int FlatScore;
    public int MultScore;

    private Rigidbody rb;
    private Vector3 initialPosition;
    private Vector3 initialRotation;

    private static bool soundPlayedThisFrame = false; // Static per-frame limiter

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

            // Play sound once per frame
            if (!soundPlayedThisFrame)
            {
                AudioSource audio = GameObject.Find("PinAudioPlayer").GetComponent<AudioSource>();
                if (audio != null && !audio.isPlaying)
                {
                    audio.Play();
                    soundPlayedThisFrame = true;
                }
            }

            HandleKnockOver();

            // Increase score by pin predefined amount
            GameManager.Instance.AddPinToScore(this);
            
        }

        // Reset static flag at end of frame
        soundPlayedThisFrame = false;
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

    protected virtual void HandleKnockOver()
    {
        // Base pins don't need special handling
    }
}