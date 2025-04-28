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

    [SerializeField]
    private ShopBackButton shopBackButton;
    
    [SerializeField]
    private ToShopButton toShopButton;

    // Per-blind state
    public int CurrentScore => CurrentScoreFlat * CurrentScoreMult;
    public int CurrentScoreMult { get; private set; } = 1;
    public int CurrentScoreFlat { get; private set; } = 0;
    public int TurnNum { get; private set; } = 0;
    public int RoundNum { get; private set; } = 0;
    public int BlindNum { get; private set; } = 0;
    public int AnteNum { get; private set; } = 0;
    public int Cash { get; private set; } = 0;
    public string ThrowType { get; private set; } = "";

    [SerializeField] private int normalBlindStartingCash = 3;

    [SerializeField] private int currentScoreToBeat = 30;

    private int strikesNum = 0;
    
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
        
        //check for turkey
        if (isStrike && strikesNum == 3)
        {
            Debug.Log("TURKEY");
            CurrentScoreMult += 2;
            ThrowType = "Turkey!";
        }

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
        
        //go to next blind if roundNum > 3
        if (RoundNum >= 3 || CurrentScore >= currentScoreToBeat)
        {
            EndBlind();
        }
        else
        {
            // Now pins are destroyed, let player select again
            PinCardManager.Instance.StartSelection();
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
        ++BlindNum;
        
        //go to next Ante if blindNum > 3
        if (BlindNum >= 3)
        {
            EndAnte();
        }
        
        // Reset RoundNum
        RoundNum = 0;
        
        //Go to results
        StartResults();
    }
    /// <summary>
    /// Ends current Ante/Lane (3 different blinds)
    /// Antes from Balatro will be called lanes
    /// </summary>
    public void EndAnte()
    {
        Debug.Log("Ante Over");

        //Increment Ante Number
        ++AnteNum;
        
        //Reset BlindNum
        BlindNum = 0;
    }

    /// <summary>
    /// Will enable everything that shows the results of the blind that the player just finished
    /// </summary>
    public void StartResults()
    {
        LayoutManager.ClearPins();
        ResultsManager.Instance.cashToBeEarned = normalBlindStartingCash + (BlindNum-1); //minus 1 because BlindNum has been incremented already
        // ResultsManager.Instance.cashToBeEarned += 3 - RoundNum; // gives more money if ended early
        ResultsManager.Instance.Enable();
    }

    public void StartShop()
    {
        // Reset Score for next blind
        CurrentScoreFlat = 0;
        CurrentScoreMult = 1;
        
        //Increase cash amount depending on which blind in this current ante/lane
        Cash += normalBlindStartingCash + (BlindNum-1);
        // Cash += 3 - RoundNum  // gives more money if ended early
        
        //enable shop back button
        shopBackButton.Enable();
        
        //set cam to look at shop spot
        mainCamera.BeginLookAtShop();
        
        GameUI.Instance.Refresh();
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
                strikesNum++;
                ThrowType = "Strike";
                return true;
            }
            // Spare if done on turn 2
            else
            {
                //shouldn't have to reset because EndTurn should do it
                Debug.Log("SPARE");
                CurrentScoreFlat += 5;
                ThrowType = "Spare";
                return false;
            }
        }
        else
        {
            Debug.Log("Normal");
            ThrowType = LayoutManager.NumPinsFallen + " Pins";
            return false;
        }
    }
}
