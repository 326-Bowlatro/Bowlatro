using Unity.VisualScripting;

using UnityEngine;

public class SpecialBallType : MonoBehaviour
{
    public enum BallType
    {
        Normal,
        Bonus100,
        ScoreMultiplier
    }

    public BallType ballType = BallType.Normal;
    private bool hasHitPin = false;
    private bool hasAppliedEffect = false;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (hasAppliedEffect) return;

        if (collision.gameObject.GetComponent<PinManager>() != null)
        {
            hasHitPin = true;

            if (ballType == BallType.Bonus100)
            {
                ScoreManager.AddScore(100);
                hasAppliedEffect = true;
            }
        }
    }

    void FixedUpdate()
    {
        if (hasAppliedEffect) return;

        if (ballType == BallType.ScoreMultiplier && 
            hasHitPin && 
            rb.linearVelocity.magnitude < 0.1f)
        {
            ApplyMultiplier();
            hasAppliedEffect = true;
        }
    }

    private void ApplyMultiplier()
    {
        int currentScore = ScoreManager.Instance.Score;
        ScoreManager.AddScore(currentScore); 
    }
}
