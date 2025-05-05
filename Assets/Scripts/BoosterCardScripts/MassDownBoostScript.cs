using UnityEngine;

public class MassDownBoostScript : MonoBehaviour
{
    private const string PLAYERTAG = "Player";
    [SerializeField] private float massAmount = .5f;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(PLAYERTAG))
        {
            // Debug.Log("Player Small");
            other.GetComponent<Rigidbody>().mass *= massAmount;
        }
    }
}
