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
    }

    public void UpdateScoreText(float flatScore, float multScore, float finalScore)
    {
        rootElement.Q<Label>("_ScoreBase").text = flatScore.ToString();
        rootElement.Q<Label>("_ScoreMult").text = multScore.ToString();
        rootElement.Q<Label>("_Score").text = finalScore.ToString();
    }
}
