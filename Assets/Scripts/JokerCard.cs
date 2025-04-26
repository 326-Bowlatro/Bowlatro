using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class JokerCardUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Joker Info")]
    public string jokerName;       // Manually set
    public int scoreBonus = 0;      // Bonus points
    public Image iconImage;         // Card icon
    public Text nameText;           // Card name

    [Header("Hover Settings")]
    public float hoverScale = 1.15f;
    private Vector3 originalScale;

    [Header("Drag Settings")]
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Vector3 startPos;
    private Transform originalParent;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        originalScale = transform.localScale;
    }

    // === Setup Joker card with info ===
    public void Setup(string name, Sprite icon, int bonus = 0)
    {
        jokerName = name;
        scoreBonus = bonus;

        if (iconImage != null) iconImage.sprite = icon;
        if (nameText != null) nameText.text = name;
    }

    // === Called when activating card ===
    public void Activate()
    {
        Debug.Log($"Activated {jokerName} (bonus: {scoreBonus})");
    }

    // === Drag Handlers ===
    public void OnBeginDrag(PointerEventData eventData)
    {
        startPos = rectTransform.position;
        originalParent = transform.parent;
        canvasGroup.blocksRaycasts = false;
        transform.SetParent(transform.root, true); // Bring to front while dragging
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // Reset if not dropped on valid DropZone
        if (transform.parent == transform.root)
        {
            rectTransform.position = startPos;
            transform.SetParent(originalParent, true);
        }

        canvasGroup.blocksRaycasts = true;
    }

    // === Hover Handlers ===
    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.localScale = originalScale * hoverScale;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.localScale = originalScale;
    }
}
