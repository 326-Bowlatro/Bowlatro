using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform Target; // Assign the ball here in the inspector
    public Vector3 Offset; // Adjustable offset to keep the camera behind the ball

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
            if (Target != null)
                transform.position = Target.position + Offset;
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
