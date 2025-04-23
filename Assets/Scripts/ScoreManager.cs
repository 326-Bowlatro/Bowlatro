using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private GameObject StrikeCard;
    [SerializeField] private TextMeshProUGUI scoreText;
    private float flatScore = 0, multScore = 1f, finalScore = 0;

    private void Start()
    {
        Pin.OnPinKnockedOver += PinOnOnPinKnockedOver;
        RoundManager.OnStrike += RoundManagerOnOnStrike;
    }

    private void RoundManagerOnOnStrike()
    {
        Instantiate(StrikeCard);
    }

    private void PinOnOnPinKnockedOver(float flat, float mult)
    {
        AddScore(flat, mult);
    }

    private void AddScore(float flat, float mult)
    {
        flatScore += flat;
        multScore += mult;
        finalScore = flatScore * multScore;
        UpdateScoreText();
    }

    private void UpdateScoreText()
    {
        scoreText.text = "Score: " + flatScore + "x" + multScore + "=" + finalScore;
    }

    private void OnDestroy()
    {
        //unsubscribe to prevent event problems
        Pin.OnPinKnockedOver -= PinOnOnPinKnockedOver;
    }
}
