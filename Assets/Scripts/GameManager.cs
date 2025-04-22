using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    // Per-round state
    // NOTE: Be careful renaming these, they're referenced directly by the .uxml
    public int roundScore;
    public int roundScoreMult;
    public int roundScoreFlat;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        BeginRound();
    }

    public void BeginRound()
    {
        // Reset per-round state
        roundScore = 0;
        roundScoreMult = 1;
        roundScoreFlat = 0;
    }

    public void NotifyPinKnockedOver(int flatScore, int multScore)
    {
        // Update score with values from Pin
        roundScoreFlat += flatScore;
        roundScoreMult += multScore;
        roundScore = roundScoreFlat * roundScoreMult;
    }
}
