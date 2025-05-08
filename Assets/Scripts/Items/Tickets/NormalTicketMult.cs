using UnityEngine;

[CreateAssetMenu(menuName = "Tickets/NormalMult")]
public class NormalTicketMult : Ticket
{
    public bool isActive = false;
    public int multFactor = 5;

    public override void Activate()
    {
        isActive = true;
    }

    public override void ApplyAffect()
    {
        if (isActive && GameManager.Instance.isNormalThrow)
        {
            GameManager.Instance.AddMultScore(multFactor);
        }
    }
}
