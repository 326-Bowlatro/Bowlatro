using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class JokerCardUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Joker Info")]
    public string jokerName;
    public int scoreBonus = 0;
    [TextArea]
    public string bonusText = "BONUS!"; // Hover tooltip message
    public Image iconImage;
    public Text nameText;

    [Header("Hover Settings")]
    public float hoverScale = 1.15f;
    private Vector3 originalScale;

    [Header("Drag Settings")]
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Vector3 startPos;
    private Transform originalParent;

    [Header("Bonus Popup (Optional)")]
    public GameObject popupPrefab;       // Floating bonus text prefab (optional)
    public Transform popupParent;        // Assign parent (e.g., Canvas)

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        originalScale = transform.localScale;
    }

    public void Setup(string name, Sprite icon, int bonus = 0)
    {
        jokerName = name;
        scoreBonus = bonus;

        if (iconImage != null) iconImage.sprite = icon;
        if (nameText != null) nameText.text = name;
    }

    public void Activate()
    {
        Debug.Log($"Activated {jokerName} (bonus: {scoreBonus})");

        // Show floating popup (optional visual feedback)
        if (popupPrefab != null && popupParent != null)
        {
            GameObject popup = Instantiate(popupPrefab, popupParent);
            TextMeshProUGUI text = popup.GetComponent<TextMeshProUGUI>();
            CanvasGroup group = popup.GetComponent<CanvasGroup>();
            if (text != null) text.text = bonusText;
            if (group != null) StartCoroutine(FadeAndDestroy(popup, group));
        }
    }

    private System.Collections.IEnumerator FadeAndDestroy(GameObject obj, CanvasGroup group)
    {
        float time = 0f;
        float duration = 1.5f;

        while (time < duration)
        {
            group.alpha = 1 - (time / duration);
            obj.transform.localPosition += Vector3.up * 30 * Time.deltaTime;
            time += Time.deltaTime;
            yield return null;
        }

        Destroy(obj);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        startPos = rectTransform.position;
        originalParent = transform.parent;
        canvasGroup.blocksRaycasts = false;
        transform.SetParent(transform.root, true);
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (transform.parent == transform.root)
        {
            rectTransform.position = startPos;
            transform.SetParent(originalParent, true);
        }

        canvasGroup.blocksRaycasts = true;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.localScale = originalScale * hoverScale;

        // 🟡 Show hover tooltip
        TooltipManager.Instance?.ShowTooltip(bonusText);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.localScale = originalScale;

        // 🟡 Hide tooltip
        TooltipManager.Instance?.HideTooltip();
    }
}
