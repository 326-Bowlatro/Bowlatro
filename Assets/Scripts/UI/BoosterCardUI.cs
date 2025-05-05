using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class BoosterCardUI : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private BoosterCardSO boostCardSO;

    [SerializeField]
    private TextMeshProUGUI cardName;

    void Start()
    {
        cardName.text = boostCardSO.boosterName;
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        UnityEngine.Debug.Log("Cicked");
        ++BoosterCardManager.Instance.chosenCardAmount;
        if (BoosterCardManager.Instance.chosenCardAmount == 2)
        {
            BoosterCardManager.Instance.EndSelection();
        }
        Destroy(gameObject);
    }
}
