using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum BossModifierManager
{
    BallVeersLeftRight,
    OnlyOddEvenPins,
    MustBounceOffWall,
    BallSlowsDown,
    MoreObstacles,
    NoStrikes
}

public class BossModifier : MonoBehaviour
{
    public BossModifierManager currentModifier;
    private int modifierIndex = 0;
    private BossModifierManager[] allModifiers;

    private BowlingBall bowlingBall;
    private Pin[] allPins;
    
    [Header("Obstacle Settings")]
    public GameObject obstaclePrefab;
    public Transform[] spawnPoints; 
    public int maxObstacles = 3;

    private readonly List<GameObject> activeObstacles = new List<GameObject>();

    void Start()
    {
        bowlingBall = FindObjectOfType<BowlingBall>();
        allPins = FindObjectsOfType<Pin>();
        allModifiers = (BossModifierManager[])System.Enum.GetValues(typeof(BossModifierManager));
    }

    public void ActivateModifierForRound()
    {
        if (GameManager.Instance.RoundNum % 3 != 0) return; // Only every 3rd round

        currentModifier = allModifiers[modifierIndex];
        modifierIndex = (modifierIndex + 1) % allModifiers.Length;

        switch (currentModifier)
        {
            case BossModifierManager.BallVeersLeftRight:
                bowlingBall.StartCoroutine(bowlingBall.VeerBall());
                break;
            case BossModifierManager.OnlyOddEvenPins:
                bool onlyOdd = Random.value > 0.5f;
                foreach (var pin in allPins)
                {
                    int pinNumber = int.Parse(pin.name.Split('_')[1]); // Assuming pins are named "Pin_1"
                    if ((onlyOdd && pinNumber % 2 == 0) || (!onlyOdd && pinNumber % 2 != 0))
                    {
                        pin.gameObject.SetActive(false);
                    }
                }
                break;
            case BossModifierManager.MustBounceOffWall:
                bowlingBall.requireBounce = true;
                break;
            case BossModifierManager.BallSlowsDown:
                bowlingBall.StartCoroutine(bowlingBall.SlowDownBall());
                break;
            case BossModifierManager.MoreObstacles:
                ActivateObstacles();
                break;
            case BossModifierManager.NoStrikes:
                // Strike prevention code
                break;
        }
    }

    public void DeactivateCurrentModifier()
    {
        // Reset any active modifications
        foreach (var pin in allPins)
        {
            pin.gameObject.SetActive(true);
        }
        
        if (bowlingBall != null)
        {
            bowlingBall.requireBounce = false;
            bowlingBall.StopAllCoroutines();
        }
    }
    
    private void ActivateObstacles()
    {
        ClearObstacles();
        SpawnObstacles();
    }

    private void SpawnObstacles()
    {
        if (spawnPoints.Length == 0 || obstaclePrefab == null)
        {
            Debug.LogWarning("Missing spawn points or obstacle prefab!");
            return;
        }

        // Create a shuffled list of available spawn points
        var availablePoints = new List<Transform>(spawnPoints);
        ShuffleList(availablePoints);

        // Spawn obstacles at random points (up to maxObstacles)
        var obstaclesToSpawn = Mathf.Min(maxObstacles, availablePoints.Count);
        for (var i = 0; i < obstaclesToSpawn; i++)
        {
            var obstacle = Instantiate(
                obstaclePrefab,
                availablePoints[i].position,
                availablePoints[i].rotation
            );
            activeObstacles.Add(obstacle);
        }
    }

    // Call this when deactivating the modifier or starting new round
    private void ClearObstacles()
    {
        foreach (var obstacle in activeObstacles.Where(obstacle => obstacle != null))
        {
            Destroy(obstacle);
        }

        activeObstacles.Clear();
    }

    //Method to shuffle list
    private static void ShuffleList<T>(List<T> list)
    {
        for (var i = 0; i < list.Count; i++)
        {
            var randomIndex = Random.Range(i, list.Count);
            (list[randomIndex], list[i]) = (list[i], list[randomIndex]);
        }
    }
}
