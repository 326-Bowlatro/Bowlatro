using System;

using UnityEngine;
using UnityEngine.Serialization;

public class CameraScript : MonoBehaviour
{
    [SerializeField] private Transform ballLookTarget;
    [SerializeField] private Vector3 camBallOffset;
    [SerializeField] private Vector3 ballLookAtOffset;

    private void Start()
    {
        transform.position = ballLookTarget.position + camBallOffset;
        transform.LookAt(ballLookTarget.position + ballLookAtOffset);
    }

}
