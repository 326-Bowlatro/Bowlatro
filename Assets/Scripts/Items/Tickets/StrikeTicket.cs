using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "Tickets/Strike")]
public class StrikeTicket : Ticket
{
    public bool isActive = false;
    public int multFactor = 10;
    private const string STRIKE = "Strike";
    public override void Activate()
    {
        isActive = true;
    }

    public override void ApplyAffect()
    {
        if (isActive && GameManager.Instance.ThrowType == STRIKE)
        {
            GameManager.Instance.AddMultScore(multFactor);
        }
    }
}
