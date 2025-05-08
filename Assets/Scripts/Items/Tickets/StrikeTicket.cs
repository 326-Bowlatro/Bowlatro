using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "Tickets/Strike")]
public class StrikeTicket : Ticket
{
    public bool isActive = false;

    public override void Activate()
    {
        isActive = true;
    }

    public override void ApplyAffect(bool boolCheck)
    {
        if (boolCheck)
        {
            GameManager.Instance.AddMultScore(10);
        }
    }
}
