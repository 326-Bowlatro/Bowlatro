using UnityEngine;

public class SellJoker : MonoBehaviour
{
    public int sellPrice = 5;

    // Temporary static cash value (for debug/testing only)
    public static int totalCash = 0;

    public void SellCard()
    {
        totalCash += sellPrice;
        Debug.Log("Sold Joker! 💰 Cash: $" + totalCash);

        Destroy(gameObject); // Destroys this JokerCard
    }
}
