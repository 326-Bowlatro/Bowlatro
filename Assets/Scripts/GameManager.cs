using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField]
    private BowlingBall bowlingBall;

    [SerializeField]
    private CameraScript cameraObject;

    // Per-round state
    public int RoundScore => RoundScoreFlat * RoundScoreMult;
    public int RoundScoreMult { get; private set; } = 1;
    public int RoundScoreFlat { get; private set; } = 0;

    void Awake()
    {
        Instance = this;
    }

    /// <summary>
    /// Calling this ends the turn and resets balls/pins.
    /// </summary>
    public void EndTurn()
    {
        // Reset ball
        bowlingBall.OnEndTurn();

        // Reset camera
        cameraObject.OnEndTurn();

        // Reset all pins
        var pins = FindObjectsByType<Pin>(FindObjectsSortMode.None).ToList();
        pins.ForEach(pin => pin.OnEndTurn());
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
