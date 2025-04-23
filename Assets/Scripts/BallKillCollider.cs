using System;
using UnityEngine;

public class BallKillCollider : MonoBehaviour
{
    private const string BALLTAG = "BowlingBall";
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(BALLTAG))
        {
            Destroy(other.gameObject);
        }
    }
}
