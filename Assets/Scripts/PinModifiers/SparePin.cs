using UnityEngine;

public class SparePin : Pin
{
    public int spareBonus = 5;

    protected override void HandleKnockOver()
    {
        base.HandleKnockOver();
        GameManager.Instance.RegisterSparePinKnockedOver(this);
    }
}