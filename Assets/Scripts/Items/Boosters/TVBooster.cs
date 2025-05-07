using UnityEngine;

[CreateAssetMenu(menuName = "Boosters/TV")]
public class TVBooster : BoosterCard
{
    public override void Activate()
    {
        GameManager.Instance.ResultsTV.isBreakable = true;
    }
    //won't have a deactivate function, can be permanent?
}
