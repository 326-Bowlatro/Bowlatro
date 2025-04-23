using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu()]
public class PinLayoutCardSO : ScriptableObject
{
    public Image image;
    public LayoutEnum layoutType;
    [SerializeField] private string layoutName;
}
