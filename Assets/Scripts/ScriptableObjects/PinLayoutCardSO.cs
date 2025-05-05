using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu()]
public class PinLayoutCardSO : ScriptableObject
{
    public GameObject prefab;
    public Image Image;
    public LayoutType LayoutType;
    public string LayoutName;

    public override string ToString() => LayoutName;
}
