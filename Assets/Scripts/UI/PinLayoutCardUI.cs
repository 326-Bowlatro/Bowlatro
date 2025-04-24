using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class PinLayoutCardUI : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private PinLayoutCardSO pinCardSO;

    [SerializeField]
    private TextMeshProUGUI cardName;

    void Start()
    {
        cardName.text = pinCardSO.LayoutName;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        GameManager.Instance.LayoutManager.SpawnPins(pinCardSO.LayoutType);
        CardManager.Instance.DisableCards();
    }
}
