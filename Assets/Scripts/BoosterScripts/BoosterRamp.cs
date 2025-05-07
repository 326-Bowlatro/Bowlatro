using UnityEngine;

public class BoosterRamp : MonoBehaviour
{
    private const string PLAYERTAG = "Player";

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag(PLAYERTAG))
        {
            GameManager.Instance.AddFlatScore(5);
        }
    }
}
