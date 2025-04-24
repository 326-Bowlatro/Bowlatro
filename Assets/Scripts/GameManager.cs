using System.Linq;
using UnityEngine;

/// <summary>
/// GameManager singleton, drives all game state.
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField]
    private BowlingBall bowlingBall;

    [SerializeField]
    private CameraScript mainCamera;

    [SerializeField] 
    private PinLayoutManager layoutManager;

    // Per-round state
    public int RoundScore => RoundScoreFlat * RoundScoreMult;
    public int RoundScoreMult { get; private set; } = 1;
    public int RoundScoreFlat { get; private set; } = 0;
    public int TurnNum { get; private set; } = 0;
    public int RoundNum { get; private set; } = 0;
    private int pinsFallen = 0;
    private int blindNum  = 0;
    public LayoutType layoutType;

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

        // Disable pins to prevent pins getting knocked down between throws
        var pins = FindObjectsByType<Pin>(FindObjectsSortMode.None).ToList();
        pins.ForEach(pin => pin.OnEndTurn());
        
        // Reset camera
        mainCamera.OnEndTurn();

        // Begin next turn and update UI
        TurnNum++;
        
        //check what kind of throw happened
        CheckForStrike();

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
        Debug.Log("Throws: " + TurnNum);
        //check if pins fallen reaches 10, since 10 is a constant number of pins
        
        //if 10 pins fell and only 1 throw is made, it's a strike
        if (pinsFallen == 10 && TurnNum == 1)
        {
            Debug.Log("STRIKE");
            EndRound();
            ++RoundScoreMult;
        }
        //if 10 pins fell and 2 throws have been made, it's a spare
        else if (pinsFallen == 10 && TurnNum == 2)
        {
            //shouldn't have to reset because EndTurn should do it
            Debug.Log("SPARE");
        }
        //if 2 throws have happened and less than 10 pins fell, then it's just normal
        else if (pinsFallen < 10 && TurnNum == 2)
        {
            //shouldn't have to reset because EndTurn should do it
            Debug.Log("Normal.");
        }
    }

    public void EndRound()
    {
        // Increment round number
        RoundNum++;

        // Reset round state
        // RoundScoreFlat = 0;
        // RoundScoreMult = 1;
        TurnNum = 0;
        pinsFallen = 0;

        // Reset all pins
        // Problem: when pins are disabled, they can't be found with this
        // var pins = FindObjectsByType<Pin>(FindObjectsSortMode.None).ToList();
        // pins.ForEach(pin => pin.OnEndRound());

        var pins = FindObjectsByType<Pin>(FindObjectsSortMode.None).ToList();
        pins.ForEach(pin => Destroy(pin.gameObject));
        //since destroying pins, just call the same function to place them after destroying all of them
        layoutManager.ChooseLayout(layoutType);

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
        //Increment blind number
        ++blindNum;
        
        //Reset Score for next blind
        RoundScoreFlat = 0;
        RoundScoreMult = 1;
        
        //reset RoundNum
        RoundNum = 0;

        //shouldn't need to reset pins, EndRound will do that
    }
    

    /// <summary>
    /// Updates the round score with values from a knocked-over pin.
    /// </summary>
    public void AddPinToScore(int flatScore, int multScore)
    {
        // Update score with values from Pin
        RoundScoreFlat += flatScore;
        RoundScoreMult += multScore;

        //called when a pin falls, so increment pins fallen count here
        ++pinsFallen;
        
        // Update UI
        GameUI.Instance.Refresh();
        
    }
}
