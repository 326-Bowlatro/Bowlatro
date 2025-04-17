using System;

using TMPro;

using Unity.VisualScripting;

using UnityEngine;
using UnityEngine.Serialization;

public class ScoreManager : MonoBehaviour
{
    // public static ScoreManager Instance;

    // [FormerlySerializedAs("Score")] public int score = 0;

    [SerializeField] private TextMeshProUGUI scoreText;
    private float flatScore = 0, multScore = 1f, finalScore = 0;
    

    private void Start()
    {
        Pin.OnPinKnockedOver += PinOnOnPinKnockedOver;
    }

    private void PinOnOnPinKnockedOver(float flat, float mult)
    {
        AddScore(flat, mult);
    }

    private void AddScore(float flat, float mult)
    {
        flatScore += flat;
        multScore += mult;
        finalScore = flatScore * multScore;
        UpdateScoreText();
    }

    private void UpdateScoreText()
    {
        scoreText.text = "Score: " + flatScore + "x" + multScore + "=" + finalScore;
    }

    private void OnDestroy()
    {
        //unsubscribe to prevent event problems
        Pin.OnPinKnockedOver -= PinOnOnPinKnockedOver;
    }
}
