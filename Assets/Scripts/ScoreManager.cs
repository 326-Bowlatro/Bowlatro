using UnityEngine;
using TMPro;  // Import the TextMeshPro namespace

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;
    public int score = 0;
    public TMP_Text scoreText;  // Change this from Text to TMP_Text

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static void AddScore(int points)
    {
        Instance.score += points;
        Instance.UpdateScoreText();
    }

    private void UpdateScoreText()
    {
        if (scoreText != null)  // Check to ensure the scoreText is not null
        {
            scoreText.text = "Score: " + Instance.score;
        }
        else
        {
            Debug.LogError("Score text component not assigned.");
        }
    }
}
