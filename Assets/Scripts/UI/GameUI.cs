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
        rootElement.Q<Label>("_Score").text = GameManager.Instance.RoundScore.ToString();
        rootElement.Q<Label>("_ScoreMult").text = GameManager.Instance.RoundScoreMult.ToString();
        rootElement.Q<Label>("_ScoreFlat").text = GameManager.Instance.RoundScoreFlat.ToString();
    }
}
