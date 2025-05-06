using UnityEngine;

[CreateAssetMenu(menuName = "Boosters/Ramp")]
public class RampBooster : BoosterCard
{
    [Header("Ramp")]
    public GameObject rampPrefab;

    private GameObject rampInstance;

    public override void Activate()
    {
        rampInstance = Instantiate(rampPrefab);
    }

    public override void Deactivate()
    {
        Destroy(rampInstance);
    }
}
