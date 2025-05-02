using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class JokerManager : MonoBehaviour
{
    public static JokerManager Instance;
    private List<string> activeJokers = new List<string>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ActivateJokers(List<string> jokerNames)
{
    activeJokers = jokerNames;
    Debug.Log("Activated Jokers: " + string.Join(", ", activeJokers));
    SceneManager.LoadScene("GameScene");
}


   public int GetTotalMultiplier(bool didStrike, bool didSpare, int pinsKnockedThisThrow)
{
    int total = 1;

    foreach (string joker in activeJokers)
    {
        switch (joker)
        {
            case "Strike":
                if (didStrike) total += 2;
                break;
            case "Spare":
                if (didSpare) total += 1;
                break;
            case "Single Throw":
                if (pinsKnockedThisThrow == 5) total += 1;
                break;
            case "Blind Boss Buff":
                if (pinsKnockedThisThrow % 2 == 1) total += 3; // odd total
                break;
            case "Ball Return Bonus":
                total += 4; // (this one just applies force later, handled elsewhere)
                break;
        }
    }

    return total;
}
public bool HasJoker(string name)
{
    return activeJokers.Contains(name);
}


}
