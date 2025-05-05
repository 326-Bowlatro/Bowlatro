using System;
using UnityEngine;

public class SizeUpBoostScript : MonoBehaviour
{
    private const string PLAYERTAG = "Player";
    [SerializeField] private float scaleAmount = 1.5f;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(PLAYERTAG))
        {
            // Debug.Log("Player Big");
            other.transform.localScale *= scaleAmount;
        }
    }
}
