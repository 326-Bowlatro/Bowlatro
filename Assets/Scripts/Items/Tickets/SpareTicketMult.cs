using UnityEngine;

[CreateAssetMenu(menuName = "Tickets/SpareMult")]
public class SpareTicketMult : Ticket
{
    public bool isActive = false;
    public int multFactor = 5;
    private const string SPARE = "Spare";

    public override void Activate()
    {
        isActive = true;
    }

    public override void ApplyAffect()
    {
        if (isActive && GameManager.Instance.ThrowType == SPARE)
        {
            GameManager.Instance.AddMultScore(multFactor);
        }
    }
}
