using UnityEngine;

[CreateAssetMenu(menuName = "Boosters/Size")]
public class SizeBooster : BoosterCard
{
    [Header("Size")]
    public float sizeFactor = 1f;

    public override void Activate()
    {
        GameManager.Instance.BowlingBall.transform.localScale *= sizeFactor;
    }
}
