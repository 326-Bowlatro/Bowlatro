using UnityEngine;

[CreateAssetMenu(menuName = "Tickets/NormalFlat")]
public class NormalTicketFlat : Ticket
{
    public bool isActive = false;
    public int flatFactor = 10;
    
    public override void Activate()
    {
        isActive = true;
    }

    public override void ApplyAffect()
    {
        if (isActive && GameManager.Instance.isNormalThrow)
        {
            GameManager.Instance.AddFlatScore(flatFactor);
        }
    }
}
