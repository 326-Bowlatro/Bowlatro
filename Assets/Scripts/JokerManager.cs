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


    public int GetTotalMultiplier()
    {
        int total = 1; // base multiplier

        foreach (string joker in activeJokers)
        {
            switch (joker)
            {
                case "Strike": total += 1; break;
                case "Single Throw": total += 0; break;
                case "Spare": total += 2; break;
                case "Blind Boss Buff": total += 3; break;
                case "Ball Return Bonus": total += 4; break;
            }
        }

        return total;
    }
}
