using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    private int flatScore = 0, multScore = 1, finalScore = 0;

    private void Start()
    {
        Pin.OnPinKnockedOver += PinOnOnPinKnockedOver;
    }

    private void PinOnOnPinKnockedOver(int flat, int mult)
    {
        AddScore(flat, mult);
    }

    private void AddScore(int flat, int mult)
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
