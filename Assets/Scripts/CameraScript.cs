using System;

using UnityEngine;
using UnityEngine.Serialization;

public class CameraScript : MonoBehaviour
{
    [SerializeField] private Transform pinLookTarget;
    [SerializeField] private Transform ballLookTarget;
    [SerializeField] private Vector3 camBallOffset;
    [SerializeField] private Vector3 camPinOffset;
    [SerializeField] private Vector3 ballLookAtOffset;
    [SerializeField] private Vector3 pinLookAtOffset;

    private bool lookAtPins;
    
    private void Start()
    {
        lookAtPins = false;
        BowlingBall.OnBallReachedPins += BowlingBallOnOnBallReachedPins;
    }

    public void OnEndTurn()
    {
        lookAtPins = false;
    }

    private void BowlingBallOnOnBallReachedPins()
    {
        lookAtPins = true;
    }

    private void Update()
    {
        if (lookAtPins)
        {
            transform.position = pinLookTarget.position + camPinOffset;
            transform.LookAt(pinLookTarget.position + pinLookAtOffset);
        }
        else
        {
            transform.position = ballLookTarget.position + camBallOffset;
            transform.LookAt(ballLookTarget.position + ballLookAtOffset);
        }
    }

    private void OnDestroy()
    {
        BowlingBall.OnBallReachedPins -= BowlingBallOnOnBallReachedPins;
    }
}
