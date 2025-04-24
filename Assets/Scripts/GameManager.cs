using System.Linq;
using UnityEngine;

/// <summary>
/// GameManager singleton, drives all game state.
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public PinLayoutManager LayoutManager;
    
    [SerializeField] private BowlingBall bowlingBall;
    [SerializeField] private CameraScript mainCamera;

    // Per-round state
    public int RoundScore => RoundScoreFlat * RoundScoreMult;
    public int RoundScoreMult { get; private set; } = 1;
    public int RoundScoreFlat { get; private set; } = 0;
    public int TurnNum { get; private set; } = 0;
    public int RoundNum { get; private set; } = 0;

    private int blindNum = 0;

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        // TODO: Move to UI button, or some input manager.
        if (Input.GetKeyDown(KeyCode.W))
        {
            bowlingBall.LaunchBall();
        }
    }

    /// <summary>
    /// Ends the player's turn and resets the ball/pins.
    /// </summary>
    public void EndTurn()
    {
        // Reset ball
        bowlingBall.OnEndTurn();

        // Reset/destroy pins (based on knocked status) to keep them from falling between throws
        LayoutManager.OnEndTurn();

        // Reset camera
        mainCamera.OnEndTurn();

        // Check what kind of throw happened
        CheckForStrike();

        // Begin next turn
        TurnNum++;

        // End the round after 2 turns
        if (TurnNum >= 2)
        {
            EndRound();
        }

        // Trigger UI refresh
        GameUI.Instance.Refresh();
    }

    private void CheckForStrike()
    {
        Debug.Log("Checking for strike...");
        Debug.Log($"Throws: {TurnNum}");

        Debug.Log($"Pins fallen: {LayoutManager.NumPinsFallen}");

        // All pins knocked?
        if (LayoutManager.NumPinsFallen == 10)
        {
            // Strike if done on turn 1
            if (TurnNum == 0)
            {
                Debug.Log("STRIKE");
                EndRound();
                ++RoundScoreMult;
            }
            // Spare if done on turn 2
            else
            {
                //shouldn't have to reset because EndTurn should do it
                Debug.Log("SPARE");
            }
        }
        else
        {
            Debug.Log("Normal");
        }
    }

    public void EndRound()
    {
        // Increment round number
        RoundNum++;

        // Reset round state
        TurnNum = 0;

        // Reset all pins
        var pins = FindObjectsByType<Pin>(FindObjectsSortMode.None).ToList();
        pins.ForEach(pin => Destroy(pin.gameObject));

        // Since pins are destroyed, just spawn a new set
        LayoutManager.SpawnLayout(LayoutManager.LayoutType);

        //go to next blind if roundNum > 3
        if (RoundNum >= 3)
        {
            EndBlind();
        }

        // Trigger UI refresh
        GameUI.Instance.Refresh();
    }

    /// <summary>
    /// Takes name from Balatro, Round will be subsections, "Blind" will be called "Game" in this, EndGame can be confusing
    /// </summary>
    public void EndBlind()
    {
        // Increment blind number
        ++blindNum;

        // Reset Score for next blind
        RoundScoreFlat = 0;
        RoundScoreMult = 1;

        // Reset RoundNum
        RoundNum = 0;
    }

    /// <summary>
    /// Updates the round score with values from a knocked-over pin.
    /// </summary>
    public void AddPinToScore(int flatScore, int multScore)
    {
        // Update score with values from Pin
        RoundScoreFlat += flatScore;
        RoundScoreMult += multScore;

        // Update UI
        GameUI.Instance.Refresh();
    }
}
