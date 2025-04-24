using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu()]
public class PinLayoutCardSO : ScriptableObject
{
    public Transform Prefab;
    public Image Image;
    public LayoutType LayoutType;
    public string LayoutName;
}
