using TMPro;
using UnityEngine;

public class PinScoreUI : MonoBehaviour
{
    [SerializeField]
    private Pin pinParent;

    [SerializeField] public Transform lookAt;
    [SerializeField] public Vector3 offset;
    private Camera cam;
    
    [SerializeField]
    private TextMeshProUGUI scoreText;

    private int getPinFlatScore()
    {
        return pinParent.FlatScore;
    }
    
    private int getPinMultScore()
    {
        return pinParent.MultScore;
    }

    private void SetPinScoreText(int flatScore, int multScore)
    {
        if (multScore == 0)
        {
            scoreText.text = "+" + flatScore;
        }
        else
        {
            scoreText.text = "+" + (flatScore * multScore);
        }
    }

    private void Start()
    {
        cam = Camera.main;
    }

    private void Update()
    {
        Vector3 position = cam.WorldToScreenPoint(lookAt.position + offset);
        
        if(transform.position != position)
            transform.position = position;
    }
    
}
