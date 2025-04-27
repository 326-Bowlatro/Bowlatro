using System.Linq;
using UnityEngine;

/// <summary>
/// GameManager singleton, drives all game state.
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public PinLayoutManager LayoutManager;

    [SerializeField]
    private BowlingBall bowlingBall;

    [SerializeField]
    private CameraScript mainCamera;

    // Per-blind state
    public int CurrentScore => CurrentScoreFlat * CurrentScoreMult;
    public int CurrentScoreMult { get; private set; } = 1;
    public int CurrentScoreFlat { get; private set; } = 0;
    public int TurnNum { get; private set; } = 0;
    public int RoundNum { get; private set; } = 0;

    private int blindNum = 0;

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            bowlingBall.LaunchBall();
        }
    }

    /// <summary>
    /// Ends the player's turn. Possibly ends the round if all pins are knocked
    /// down, or if we've hit the turn limit (2).
    /// </summary>
    public void EndTurn()
    {
        Debug.Log("Turn over");

        // Reset ball
        bowlingBall.OnEndTurn();

        // Reset/destroy pins (based on knocked status) to keep them from falling between throws
        LayoutManager.OnEndTurn();

        // Reset camera
        mainCamera.OnEndTurn();

        // Check what kind of throw happened
        bool isStrike = CheckForStrike();

        // Begin next turn
        TurnNum++;

        // End the round after 2 turns or a strike
        if (TurnNum >= 2 || isStrike)
        {
            EndRound();
        }

        // Trigger UI refresh
        GameUI.Instance.Refresh();
    }

    /// <summary>
    /// Ends the current round, resetting pins. Possibly ends the blind if we've
    /// hit the round limit (3).
    /// </summary>
    public void EndRound()
    {
        Debug.Log("Round over");

        // Increment round number
        RoundNum++;

        // Reset round state
        TurnNum = 0;

        // Destroy all existing pins
        LayoutManager.ClearPins();

        // Now pins are destroyed, replace with a new set
        LayoutManager.SpawnPins(LayoutManager.LayoutType);

        //go to next blind if roundNum > 3
        if (RoundNum >= 3)
        {
            EndBlind();
        }

        // Trigger UI refresh
        GameUI.Instance.Refresh();
    }

    /// <summary>
    /// Ends the current blind (3 rounds).
    /// Takes name from Balatro, Round will be subsections, "Blind" will be called "Game" in this, EndGame can be confusing
    /// </summary>
    public void EndBlind()
    {
        Debug.Log("Blind over");

        // Increment blind number
        ++blindNum;

        // Reset Score for next blind
        CurrentScoreFlat = 0;
        CurrentScoreMult = 1;

        // Reset RoundNum
        RoundNum = 0;
    }

    /// <summary>
    /// Updates the blind score with values from a knocked-over pin.
    /// </summary>
    public void AddPinToScore(Pin pin)
    {
        int jokerMultiplier =
            JokerManager.Instance != null ? JokerManager.Instance.GetTotalMultiplier() : 1;

        // Update score with values from Pin
        CurrentScoreFlat += pin.FlatScore * jokerMultiplier;
        CurrentScoreMult += pin.MultScore;

        // Update UI
        GameUI.Instance.Refresh();
    }

    /// <summary>
    /// Checks if the player has scored a strike.
    /// </summary>
    private bool CheckForStrike()
    {
        // All pins knocked?
        if (LayoutManager.NumPinsFallen == 10)
        {
            // Strike if done on turn 1
            if (TurnNum == 0)
            {
                Debug.Log("STRIKE");

                CurrentScoreMult++;
                return true;
            }
            // Spare if done on turn 2
            else
            {
                //shouldn't have to reset because EndTurn should do it
                Debug.Log("SPARE");
                return false;
            }
        }
        else
        {
            Debug.Log("Normal");
            return false;
        }
    }
}
