using UnityEngine;

public class BallMenu : MonoBehaviour
{
    public GameObject normalBallPrefab;
    public GameObject bonus100BallPrefab;
    public GameObject multiplierBallPrefab;
    
    public Transform spawnPoint;
    private GameObject currentBall;

    public void SelectNormalBall()
    {
        SpawnBall(normalBallPrefab);
    }

    public void SelectBonus100Ball()
    {
        SpawnBall(bonus100BallPrefab);
    }

    public void SelectMultiplierBall()
    {
        SpawnBall(multiplierBallPrefab);
    }

    private void SpawnBall(GameObject prefab)
    {
        if (currentBall != null)
        {
            Destroy(currentBall);
        }
        
        currentBall = Instantiate(prefab, spawnPoint.position, spawnPoint.rotation);
    }
}

