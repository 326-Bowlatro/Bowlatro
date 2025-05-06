using System;
using System.Collections.Generic;
using UnityEngine;

public class BoosterManager : MonoBehaviour
{
    [SerializeField] private GameObject ball;
    [SerializeField] private GameObject ramp;
    [SerializeField] private float massUpFactor = 3f;
    [SerializeField] private float massDownFactor = .5f;
    [SerializeField] private float sizeUpFactor = 1.25f;
    [SerializeField] private float sizeDownFactor = .75f;

    [SerializeField] private List<GameObject> boostObjects;
    public enum BoostType
    {
        SizeUp,
        SizeDown,
        MassUp,
        MassDown,
        Ramp
    }
    public void ApplyAffect(BoostType boostType)
    {
        switch (boostType)
        {
            case BoostType.SizeUp:
                ball.transform.localScale *= sizeUpFactor;
                break;
            case BoostType.SizeDown:
                ball.transform.localScale *= sizeDownFactor;
                break;
            case BoostType.MassUp:
                ball.GetComponent<Rigidbody>().mass *= massUpFactor;
                break;
            case BoostType.MassDown:
                ball.GetComponent<Rigidbody>().mass *= massDownFactor;
                break;
            case BoostType.Ramp:
                GameObject rampBoost = Instantiate(ramp);
                boostObjects.Add(rampBoost);
                break;
            default:
                break;
        }
    }

    public void DestroyBoosts()
    {
        foreach (GameObject boost in boostObjects)
        {
            Destroy(boost);
        }
        boostObjects.Clear();
    }
    
}
