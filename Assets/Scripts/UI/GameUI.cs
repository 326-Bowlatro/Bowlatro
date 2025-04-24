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
    }
}
