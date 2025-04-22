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

    private bool isLookingAtPins = false;

    /// <summary>
    /// Switches camera mode to show the pins.
    /// </summary>
    public void BeginLookAtPins()
    {
        isLookingAtPins = true;
    }

    /// <summary>
    /// Switches camera mode to follow the ball.
    /// </summary>
    public void EndLookAtPins()
    {
        isLookingAtPins = false;
    }

    public void OnEndTurn()
    {
        EndLookAtPins();
    }

    private void Update()
    {
        if (isLookingAtPins)
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
}
