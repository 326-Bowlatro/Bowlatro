using UnityEngine;
using UnityEngine.Serialization;

public class CameraFollow : MonoBehaviour
{
    [FormerlySerializedAs("Target")] public Transform ballTransform;
    [FormerlySerializedAs("Offset")] public Vector3 cameraOffset;

    private Vector3 initialPosition;
    private bool shouldFollow = true;

    void Start()
    {
        initialPosition = transform.position;
    }

    void Update()
    {
        if (shouldFollow)
        {
            if (ballTransform != null)
                transform.position = ballTransform.position + cameraOffset;
        }

        // Toggle follow/reset based on a condition, e.g., the ball has hit the pins
        if (Input.GetKeyDown(KeyCode.T)) // Press 'T' to toggle follow/reset for testing
        {
            shouldFollow = !shouldFollow;
            if (!shouldFollow)
                ResetCamera();
        }
    }

    public void ResetCamera()
    {
        transform.position = initialPosition; // Reset camera to initial position
    }
}
