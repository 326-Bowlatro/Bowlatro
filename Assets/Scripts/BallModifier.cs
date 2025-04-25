using UnityEngine;

public class BallModifier : MonoBehaviour
{
    public enum BallType
    {
        Normal,
        Multiplier,
        Bonus
    }

    public BallType type;
    public GameObject visualEffect;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            var bowlingBall = collision.gameObject.GetComponent<BowlingBall>();
            
            if (type == BallType.Multiplier)
            {
                bowlingBall.isMultiplierBall = true;
            }
            else if (type == BallType.Bonus)
            {
                bowlingBall.isBonusBall = true;
            }

            if (visualEffect != null)
            {
                Instantiate(visualEffect, transform.position, Quaternion.identity);
            }
            
            gameObject.SetActive(false);
        }
    }
}
