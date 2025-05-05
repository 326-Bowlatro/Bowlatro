using UnityEngine;

public class SizeDownBoostScript : MonoBehaviour
{
    private const string PLAYERTAG = "Player";
    [SerializeField] private float scaleAmount = .75f;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(PLAYERTAG))
        {
            // Debug.Log("Player Small");
            other.transform.localScale *= scaleAmount;
            GameManager.Instance.ballSizeDown = true;
        }
    }
}
