using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BossModifier
{
    VeerLeft,
    VeerRight,
    SlowDown,
    Obstacles,
    NoStrike
}

public class BossModifierManager : MonoBehaviour
{
    [Header("Settings")]
    public int blindsPerBoss = 2; // Boss every 3 blinds
    public GameObject obstaclePrefab;
    public Transform[] obstacleSpawnPoints;

    [Header("Current State")]
    public bool isBossActive = false;
    public BossModifier currentModifier;
    
    private List<BossModifier> unusedModifiers = new List<BossModifier>();
    private readonly List<BossModifier> usedModifiers = new List<BossModifier>();
    private GameObject[] spawnedObstacles;
    private int blindCounter = 0;

    void Awake()
    {
        InitializeModifiers();
    }

    private void InitializeModifiers()
    {
        unusedModifiers = new List<BossModifier>((BossModifier[])System.Enum.GetValues(typeof(BossModifier)));
        usedModifiers.Clear();
    }

    public void OnBlindCompleted()
    {
        blindCounter++;
        
        if (blindCounter % blindsPerBoss == 0)
        {
            StartBossModifier();
        }
        else
        {
            EndBossModifier();
        }
    }

    private void StartBossModifier()
    {
        if (unusedModifiers.Count == 0)
        {
            InitializeModifiers();
        }

        int randomIndex = Random.Range(0, unusedModifiers.Count);
        currentModifier = unusedModifiers[randomIndex];
        unusedModifiers.RemoveAt(randomIndex);
        
        
        GameManager.Instance.BowlingBall.SetBossBlind(true);
        isBossActive = true;
        ApplyModifier(currentModifier);
        
        Debug.Log($"BOSS MODIFIER ACTIVATED (Blind {blindCounter}): {currentModifier}");
    }

    private void ApplyModifier(BossModifier modifier)
    {
        switch (modifier)
        {
            case BossModifier.VeerLeft:
                GameManager.Instance.BowlingBall.SetVeerEnabled(true, 1f);
                break;
                
            case BossModifier.VeerRight:
                GameManager.Instance.BowlingBall.SetVeerEnabled(true, -1f);
                break;
                
            case BossModifier.SlowDown:
                GameManager.Instance.BowlingBall.SetSlowDownEnabled(true);
                break;
                
            case BossModifier.Obstacles:
                SpawnObstacles();
                break;
                
            case BossModifier.NoStrike:
                // Handled in GameManager's strike check
                break;
        }
    }

    private void EndBossModifier()
    {
        if (!isBossActive) return;
        
        ClearCurrentModifier();
        isBossActive = false;

    }

    private void ClearCurrentModifier()
    {
        GameManager.Instance.BowlingBall.SetVeerEnabled(false, 0f);
        GameManager.Instance.BowlingBall.SetSlowDownEnabled(false);
        GameManager.Instance.BowlingBall.StopAllCoroutines();
        RemoveObstacles();
    }


    private void SpawnObstacles()
    {
        RemoveObstacles();
        spawnedObstacles = new GameObject[obstacleSpawnPoints.Length];
        
        for (int i = 0; i < obstacleSpawnPoints.Length; i++)
        {
            spawnedObstacles[i] = Instantiate(
                obstaclePrefab,
                obstacleSpawnPoints[i].position,
                obstacleSpawnPoints[i].rotation
            );
        }
    }

    private void RemoveObstacles()
    {
        if (spawnedObstacles == null) return;
        foreach (var obstacle in spawnedObstacles)
        {
            if (obstacle != null) Destroy(obstacle);
        }
    }
}