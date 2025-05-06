using UnityEngine;

[CreateAssetMenu(menuName = "Boosters/Bonus")]
public class BonusBooster : BoosterCard
{
    [Header("Bonus")]
    public int FlatBonus = 0;
    public int MultBonus = 0;

    public override void Activate()
    {
        // TODO
    }
}
