using UnityEngine;

[CreateAssetMenu(menuName = "Boosters/RollingPin")]
public class RollingPinBooster : BoosterCard
{
    [Header("Width")]
    public float widthAmount = 3.5f;

    public override void Activate()
    {
        Vector3 scale = GameManager.Instance.BowlingBall.transform.localScale;
        GameManager.Instance.BowlingBall.transform.localScale = new Vector3(widthAmount, scale.y, scale.z);
    }
}
