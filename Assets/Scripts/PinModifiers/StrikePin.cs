using UnityEngine;

public class StrikePin : Pin
{
    public int strikeBonus = 10;

    protected override void HandleKnockOver()
    {
        base.HandleKnockOver();
        GameManager.Instance.RegisterStrikePinKnockedOver(this);
    }
}

