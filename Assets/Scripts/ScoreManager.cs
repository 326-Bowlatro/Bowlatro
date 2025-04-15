using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    public int Score = 0;
    public TextMeshProUGUI ScoreText;

    void Awake()
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
        Instance.Score += points;
        Instance.UpdateScoreText();
    }

    private void UpdateScoreText()
    {
        if (ScoreText != null) // Check to ensure the scoreText is not null
        {
            ScoreText.text = "Score: " + Instance.Score;
        }
        else
        {
            Debug.LogError("Score text component not assigned.");
        }
    }
}
