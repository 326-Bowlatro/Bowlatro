using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    // Per-round state
    public int RoundScore => RoundScoreFlat * RoundScoreMult;
    public int RoundScoreMult { get; private set; } = 1;
    public int RoundScoreFlat { get; private set; } = 0;

    void Awake()
    {
        Instance = this;
    }

    public void NotifyPinKnockedOver(int flatScore, int multScore)
    {
        // Update score with values from Pin
        RoundScoreFlat += flatScore;
        RoundScoreMult += multScore;

        // Update UI
        GameUI.Instance.Refresh();
    }
}
