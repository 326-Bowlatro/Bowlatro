using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    private float flatScore = 0, multScore = 1f, finalScore = 0;

    private void Start()
    {
        Pin.OnPinKnockedOver += PinOnOnPinKnockedOver;
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
        GameUI.Instance.UpdateScoreText(flatScore, multScore, finalScore);
    }

    private void OnDestroy()
    {
        //unsubscribe to prevent event problems
        Pin.OnPinKnockedOver -= PinOnOnPinKnockedOver;
    }
}
