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
        ++BoosterCardManager.Instance.chosenCardAmount;
        GameManager.Instance.BoosterManager.ApplyAffect(boostCardSO.boostType);
        if (BoosterCardManager.Instance.chosenCardAmount >= 2)
        {
            BoosterCardManager.Instance.EndSelection();
        }
        Destroy(gameObject);
    }
}
