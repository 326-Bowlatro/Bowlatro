using UnityEngine;

[CreateAssetMenu(menuName = "Tickets/SpareFlat")]
public class SpareTicketFlat : Ticket
{
    public bool isActive = false;
    public int flatFactor = 10;
    private const string SPARE = "Spare";
    
    public override void Activate()
    {
        isActive = true;
    }

    public override void ApplyAffect()
    {
        if (isActive && GameManager.Instance.ThrowType == SPARE)
        {
            GameManager.Instance.AddFlatScore(flatFactor);
        }
    }
}
