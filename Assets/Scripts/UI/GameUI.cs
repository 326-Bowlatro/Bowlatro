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

        // Set data source to self
        rootElement.dataSource = this;
    }

    public void UpdateScoreText(int flatScore, int multScore, int finalScore)
    {
        rootElement.Q<Label>("_ScoreFlat").text = flatScore.ToString();
        rootElement.Q<Label>("_ScoreMult").text = multScore.ToString();
        rootElement.Q<Label>("_Score").text = finalScore.ToString();
    }
}
