using TMPro;
using UnityEngine;

public class GameUI : MonoBehaviour
{
    public static GameUI Instance { get; private set; }

    // Element references
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI scoreMultText;
    [SerializeField] private TextMeshProUGUI scoreFlatText;
    [SerializeField] private TextMeshProUGUI turnText;
    [SerializeField] private TextMeshProUGUI roundText;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        Refresh();
    }

    /// <summary>
    /// Refreshes entire UI with latest state. Should be called any time UI-visible values are changed.
    /// </summary>
    public void Refresh()
    {
        scoreText.text = GameManager.Instance.CurrentScore.ToString();
        scoreMultText.text = GameManager.Instance.CurrentScoreMult.ToString();
        scoreFlatText.text = GameManager.Instance.CurrentScoreFlat.ToString();
        turnText.text = (GameManager.Instance.TurnNum + 1).ToString();
        roundText.text = (GameManager.Instance.RoundNum + 1).ToString();
    }
}
