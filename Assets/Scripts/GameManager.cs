using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// GameManager singleton, drives all game state.
/// </summary>
public class GameManager : StateMachine<GameManager, GameManager.PreRoundState>
{
    public static GameManager Instance { get; private set; }

    public PinLayoutManager LayoutManager => pinLayoutManager;
    public BowlingBall BowlingBall => bowlingBall;
    public ShopManager ShopManager => shopManager;
    public InventoryManager InventoryManager => inventoryManager;

    public PinLayoutCard SelectedLayout { get; set; } = null;

    [SerializeField]
    private BowlingBall bowlingBall;

    [SerializeField]
    private CameraScript mainCamera;

    [SerializeField]
    private ToShopButton toShopButton;

    [SerializeField]
    private int normalBlindStartingCash = 3;

    // Per-blind state
    public int CurrentScore => CurrentScoreFlat * CurrentScoreMult;
    public int CurrentScoreMult { get; private set; } = 1;
    public int CurrentScoreFlat { get; private set; } = 0;
    public int TurnNum { get; private set; } = 0;
    public int RoundNum { get; private set; } = 0;
    public int BlindNum { get; private set; } = 0;
    public int AnteNum { get; private set; } = 0;
    public int Cash { get; private set; } = 3;
    public string ThrowType { get; private set; } = "";
    public bool IsBossStage { get; private set; } = false;
    public int CurrentScoreToBeat { get; private set; } = 20;
    public int CurrentBossScoreToBeat { get; private set; }

    private int strikesNum = 0;
    private BossModifierManager bossModifierManager;
    private PinLayoutManager pinLayoutManager;
    private ShopManager shopManager;
    private InventoryManager inventoryManager;

    private readonly List<StrikePin> knockedOverStrikePins = new();
    private readonly List<SparePin> knockedOverSparePins = new();

    private int cashToBeEarned = 0;

    void Awake()
    {
        Instance = this;
        CurrentBossScoreToBeat += CurrentScoreToBeat + CurrentScoreToBeat / 2;

        bossModifierManager = GetComponent<BossModifierManager>();
        pinLayoutManager = GetComponent<PinLayoutManager>();
        shopManager = GetComponent<ShopManager>();
        inventoryManager = GetComponent<InventoryManager>();
    }

    /// <summary>
    /// Behavior specific to "pre-round" state (choosing a hand).
    /// </summary>
    public class PreRoundState : State
    {
        public override void EnterState()
        {
            // Check for boss stage here, so it will check once you leave the shop
            if ((Self.BlindNum + 1) % 3 == 0 && Self.BlindNum > 0)
            {
                Self.IsBossStage = true;
            }
            else
            {
                Self.IsBossStage = false;
            }

            // Request a new hand
            Self.InventoryManager.ResetHand();

            // Clear layout slot
            Self.SelectedLayout = null;
        }

        public override void ExitState() { }
    }

    /// <summary>
    /// Behavior specific to "playing" state.
    /// </summary>
    public class PlayingState : State
    {
        public override void EnterState()
        {
            // Spawn pins from selected layout
            Self.LayoutManager.SpawnPins(Self.SelectedLayout.LayoutType);
        }

        public override void ExitState() { }

        public override void UpdateState()
        {
            var ball = Self.bowlingBall;

            if (Input.GetKeyDown(KeyCode.W))
            {
                ball.LaunchBall();
            }

            if (!ball.HasLaunched)
            {
                ball.ProcessMovement();
            }
        }
    }

    /// <summary>
    /// Behavior specific to "results" state.
    /// </summary>
    public sealed class ResultsState : State
    {
        public override void EnterState()
        {
            // Boss stage gives extra
            if (Self.IsBossStage)
            {
                Self.cashToBeEarned = Self.normalBlindStartingCash * 2;
            }
            else
            {
                Self.cashToBeEarned = Self.normalBlindStartingCash + (Self.BlindNum - 1); //minus 1 because BlindNum has been incremented already
            }

            // Reset score for next blind
            Self.CurrentScoreFlat = 0;
            Self.CurrentScoreMult = 1;

            // Boss stage gives 2x cash. Else, multiplier is based on blind number.
            Self.Cash += Self.IsBossStage
                ? Self.normalBlindStartingCash * 2
                : Self.normalBlindStartingCash + (Self.BlindNum - 1);

            // Have interest, every 10, get 1
            Self.Cash += Self.Cash % 10;

            // Clear all pins
            Self.LayoutManager.ClearPins();

            // Move camera, show shop button
            Self.mainCamera.BeginLookAtResults();
            Self.toShopButton.Enable();
            Debug.Log(Self.cashToBeEarned);
        }

        public override void ExitState()
        {
            Self.mainCamera.EndLookAtResults();
            Self.toShopButton.Disable();
        }
    }

    /// <summary>
    /// Behavior specific to "shop" state.
    /// </summary>
    public class ShopState : State
    {
        public bool IsOpeningPack { get; set; } = false;

        public override void EnterState()
        {
            // Set cam to look at shop spot
            Self.mainCamera.BeginLookAtShop();

            // Reset/init shop inventory
            Self.shopManager.ResetInventory();
        }

        public override void ExitState()
        {
            Self.mainCamera.EndLookAtShop();

            // Check for boss stage here, so it will check once you leave the shop
            if ((Self.BlindNum + 1) % 3 == 0 && Self.BlindNum > 0)
            {
                Self.IsBossStage = true;
            }
            else
            {
                Self.IsBossStage = false;
            }
        }
    }

    /// <summary>
    /// Updates the blind score with values from a knocked-over pin.
    /// </summary>
    public void AddPinToScore(Pin pin)
    {
        // Function can only be called in "playing" state.
        AssertState<PlayingState>();

        int jokerMultiplier =
            JokerManager.Instance != null ? JokerManager.Instance.GetTotalMultiplier() : 1;

        // Update score with values from Pin
        CurrentScoreFlat += pin.FlatScore * jokerMultiplier;
        CurrentScoreMult += pin.MultScore;

        // Update UI
        GameUI.Instance.Refresh();
    }

    /// <summary>
    /// Ends the player's turn. Possibly ends the round if all pins are knocked
    /// down, or if we've hit the turn limit (2).
    /// </summary>
    public void EndTurn()
    {
        // Function can only be called in "playing" state.
        AssertState<PlayingState>();

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

        //if score is reached, end blind early
        if (
            (CurrentScore >= CurrentScoreToBeat && !IsBossStage)
            || (IsBossStage && CurrentScore >= CurrentBossScoreToBeat)
            || true /* TEMP DO NOT COMMIT ME! */
        )
        {
            EndBlind();

            //End round early
            bowlingBall.OnEndTurn();
            TurnNum = 0;
        }
        // End the round after 2 turns or a strike
        else if (TurnNum >= 2 || isStrike)
        {
            EndRound();
        }

        GameUI.Instance.Refresh();
    }

    /// <summary>
    /// Ends the current round, resetting pins. Possibly ends the blind if we've
    /// hit the round limit (3).
    /// </summary>
    public void EndRound()
    {
        // Function can only be called in "playing" state.
        AssertState<PlayingState>();

        Debug.Log("Round over");

        // Increment round number
        RoundNum++;

        // Reset round state
        TurnNum = 0;
        ThrowType = "";

        // Destroy all existing pins
        LayoutManager.ClearPins();

        //go to next blind if roundNum > 3
        if (RoundNum >= 3)
        {
            EndBlind();
        }
        else
        {
            // Transition back to pre-round state
            GoToState<PreRoundState>();
        }
    }

    /// <summary>
    /// Ends the current blind (3 rounds).
    /// Takes name from Balatro, Round will be subsections, "Blind" will be called "Game" in this, EndGame can be confusing
    /// </summary>
    public void EndBlind()
    {
        // Function can only be called in "playing" state.
        AssertState<PlayingState>();

        Debug.Log("Blind over");

        // Notify boss modifier manager
        bossModifierManager.OnBlindCompleted();

        BlindNum++;

        //check if score is enough, if not, you lose
        if (CurrentScore < CurrentScoreToBeat)
        {
            Debug.Log("Lose.");
        }

        //go to next Ante if blindNum > 3
        if (BlindNum >= 3)
        {
            EndAnte();
        }

        // Reset RoundNum
        RoundNum = 0;

        // Reset strike count
        strikesNum = 0;

        // Increase score to beat
        CurrentScoreToBeat += CurrentScoreToBeat / 2;
        CurrentBossScoreToBeat += CurrentScoreToBeat / 2;

        // Go to results
        GoToState<ResultsState>();
    }

    /// <summary>
    /// Ends current Ante/Lane (3 different blinds)
    /// Antes from Balatro will be called lanes
    /// </summary>
    public void EndAnte()
    {
        // Function can only be called in "playing" state.
        AssertState<PlayingState>();

        Debug.Log("Ante Over");

        //Increment Ante Number
        AnteNum++;

        //Reset BlindNum
        BlindNum = 0;
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

                if (
                    bossModifierManager.isBossActive
                    && bossModifierManager.currentModifier == BossModifier.NoStrike
                )
                {
                    Debug.Log("STRIKE PREVENTED by NoStrike modifier");
                    foreach (var pin in knockedOverStrikePins)
                    {
                        CurrentScoreMult += pin.strikeBonus;
                        Debug.Log($"Strike Pin applied +{pin.strikeBonus} despite NoStrike");
                    }
                    knockedOverStrikePins.Clear();

                    return true;
                }

                foreach (var pin in LayoutManager.Pins)
                {
                    if (pin is StrikePin && pin.IsKnockedOver)
                    {
                        CurrentScoreMult += ((StrikePin)pin).strikeBonus;
                        Debug.Log(
                            $"Strike Pin Bonus! +{((StrikePin)pin).strikeBonus} to multiplier!"
                        );
                        break;
                    }
                }

                CurrentScoreMult++;
                strikesNum++;
                ThrowType = "Strike";
                foreach (var pin in knockedOverStrikePins)
                {
                    CurrentScoreMult += pin.strikeBonus;
                    Debug.Log($"Strike Pin applied +{pin.strikeBonus} despite NoStrike");
                }
                knockedOverStrikePins.Clear();

                return true;
            }
            // Spare if done on turn 2
            else
            {
                //shouldn't have to reset because EndTurn should do it
                Debug.Log("SPARE");
                foreach (var pin in knockedOverSparePins)
                {
                    CurrentScoreFlat += pin.spareBonus;
                    Debug.Log($"Spare Pin bonus: +{pin.spareBonus}");
                }
                knockedOverSparePins.Clear();

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

    /// <summary>
    /// Deducts some amount of cash, or throws if the amount is unaffordable.
    /// </summary>
    public void DeductCash(int amount)
    {
        if (Cash - amount < 0)
        {
            throw new System.Exception("Not enough cash available to deduct!");
        }

        Cash -= amount;
        Debug.Log($"Spent ${amount}, player has ${Cash} remaining.");
    }

    public void RegisterStrikePinKnockedOver(StrikePin pin)
    {
        knockedOverStrikePins.Add(pin);
    }

    public void RegisterSparePinKnockedOver(SparePin pin)
    {
        knockedOverSparePins.Add(pin);
    }

    public void ClearKnockedOverPins()
    {
        knockedOverStrikePins.Clear();
        knockedOverSparePins.Clear();
    }
}
