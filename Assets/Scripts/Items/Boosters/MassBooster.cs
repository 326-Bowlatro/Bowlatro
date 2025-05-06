using UnityEngine;

[CreateAssetMenu(menuName = "Boosters/Mass")]
public class MassBooster : BoosterCard
{
    [Header("Mass")]
    public float massFactor = 1f;

    public override void Activate()
    {
        GameManager.Instance.BowlingBall.GetComponent<Rigidbody>().mass *= massFactor;
    }
}
