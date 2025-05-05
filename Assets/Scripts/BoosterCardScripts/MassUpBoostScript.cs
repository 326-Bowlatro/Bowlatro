using UnityEngine;

public class MassUpBoostScript : MonoBehaviour
{
    private const string PLAYERTAG = "Player";
    [SerializeField] private float massAmount = 3;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(PLAYERTAG))
        {
            // Debug.Log("Player Small");
            other.GetComponent<Rigidbody>().mass *= massAmount;
        }
    }
}
