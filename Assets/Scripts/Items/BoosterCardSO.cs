using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu()]
public class BoosterCardSO : ScriptableObject
{
    public GameObject prefab;
    public GameObject boostPrefab;
    public BoosterManager.BoostType boostType;
    public Sprite icon;
    public string boosterName;
}
