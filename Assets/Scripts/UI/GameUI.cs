using UnityEngine;
using UnityEngine.UIElements;

public class GameUI : MonoBehaviour
{
    public static GameUI Instance { get; private set; }

    private VisualElement rootElement;

    void Awake()
    {
        Instance = this;
    }

    void OnEnable()
    {
        // Fetch root element from UIDocument
        var uiDocument = GetComponent<UIDocument>();
        rootElement = uiDocument.rootVisualElement;

        // Setup button handlers
        rootElement.Q<Button>("_ShopContinue").clicked += () =>
        {
            // Go to playing state
            GameManager.Instance.GoToState<GameManager.PlayingState>();
        };

        Refresh();
    }

    /// <summary>
    /// Refreshes entire UI with latest state. Should be called any time UI-visible values are changed.
    /// </summary>
    public void Refresh()
    {
        rootElement.Q<Label>("_Score").text = GameManager.Instance.CurrentScore.ToString();
        rootElement.Q<Label>("_ScoreMult").text = GameManager.Instance.CurrentScoreMult.ToString();
        rootElement.Q<Label>("_ScoreFlat").text = GameManager.Instance.CurrentScoreFlat.ToString();
        rootElement.Q<Label>("_Turn").text = (GameManager.Instance.TurnNum + 1).ToString();
        rootElement.Q<Label>("_Round").text = (GameManager.Instance.RoundNum + 1).ToString();

        rootElement.Q<Label>("_Wallet").text = "$" + GameManager.Instance.Cash;
        rootElement.Q<Label>("_Status").text = GameManager.Instance.ThrowType;

        // Update stage label
        if ((GameManager.Instance.BlindNum + 1) % 3 == 0)
        {
            rootElement.Q<Label>("_Stage").text = "Boss Stage";
        }
        else
        {
            rootElement.Q<Label>("_Stage").text = "Stage " + (GameManager.Instance.BlindNum + 1);
        }

        // Update score to beat
        rootElement.Q<Label>("_ScoreToBeat").text = GameManager.Instance.IsBossStage
            ? GameManager.Instance.CurrentBossScoreToBeat.ToString()
            : GameManager.Instance.CurrentScoreToBeat.ToString();

        // Update shop visibility (use translation to animate show/hide)
        var shopHost = rootElement.Q<VisualElement>("_ShopHost");
        shopHost.style.translate = new StyleTranslate(
            new Translate(0, GameManager.Instance.CurrentState is GameManager.ShopState ? 0 : -340)
        );
    }
}
