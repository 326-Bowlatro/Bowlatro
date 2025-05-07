using System;
using UnityEngine;

public class ResultsTV : MonoBehaviour
{
    private const string PLAYERTAG = "Player";
    public bool isBreakable = false;
    [SerializeField] private int points = 5;
    private void OnCollisionEnter(Collision other)
    {
        if (isBreakable && other.gameObject.CompareTag(PLAYERTAG))
        {
            GameManager.Instance.AddFlatScore(points);
        }
    }
}
