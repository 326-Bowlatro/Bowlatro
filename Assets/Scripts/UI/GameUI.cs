using UnityEngine;
using UnityEngine.UIElements;
using static GameManager;

public class GameUI : MonoBehaviour
{
    public static GameUI Instance { get; private set; }

    private VisualElement rootElement;

    void Awake()
    {
        Instance = this;
    }

    void OnEnable()
    {
        // Fetch root element from UIDocument
        var uiDocument = GetComponent<UIDocument>();
        rootElement = uiDocument.rootVisualElement;

        // Setup button handlers (left panel)
        rootElement.Q<Button>("_LeftPanel_Exit").clicked += () =>
        {
#if UNITY_EDITOR
            // Exit play mode in the editor
            UnityEditor.EditorApplication.isPlaying = false;
#else
            // Exit application in the final build
            Application.Quit();
#endif
        };

        // Setup button handlers (shop)
        rootElement.Q<Button>("_Shop_Continue").clicked += () =>
        {
            // Go to playing state
            GameManager.Instance.GoToState<PlayingState>();
        };
        rootElement.Q<Button>("_Shop_Reroll").clicked += () =>
        {
            // Spend cash, reset shop. Ignore if not enough cash.
            if (GameManager.Instance.Cash >= ShopManager.RerollCost)
            {
                GameManager.Instance.DeductCash(ShopManager.RerollCost);
                GameManager.Instance.ShopManager.ResetInventory();
            }
        };
        rootElement.Q<Button>("_Shop_ItemPackButton").clicked += () =>
        {
            (GameManager.Instance.CurrentState as ShopState).IsOpeningPack = true;
            Refresh();
        };
        rootElement.Q<Button>("_ShopPackCancel").clicked += () =>
        {
            (GameManager.Instance.CurrentState as ShopState).IsOpeningPack = false;
            Refresh();
        };

        Refresh();
    }

    /// <summary>
    /// Refreshes entire UI with latest state. Should be called any time UI-visible values are changed.
    /// </summary>
    public void Refresh()
    {
        rootElement.Q<Label>("_Score").text = GameManager.Instance.CurrentScore.ToString();
        rootElement.Q<Label>("_ScoreMult").text = GameManager.Instance.CurrentScoreMult.ToString();
        rootElement.Q<Label>("_ScoreFlat").text = GameManager.Instance.CurrentScoreFlat.ToString();
        rootElement.Q<Label>("_Turn").text = (GameManager.Instance.TurnNum + 1).ToString();
        rootElement.Q<Label>("_Round").text = (GameManager.Instance.RoundNum + 1).ToString();

        rootElement.Q<Label>("_Wallet").text = "$" + GameManager.Instance.Cash;
        rootElement.Q<Label>("_Status").text = GameManager.Instance.ThrowType;

        // Update stage label
        if ((GameManager.Instance.BlindNum + 1) % 3 == 0)
        {
            rootElement.Q<Label>("_Stage").text = "Boss Stage";
        }
        else
        {
            rootElement.Q<Label>("_Stage").text = "Stage " + (GameManager.Instance.BlindNum + 1);
        }

        // Update score to beat
        rootElement.Q<Label>("_ScoreToBeat").text = GameManager.Instance.IsBossStage
            ? GameManager.Instance.CurrentBossScoreToBeat.ToString()
            : GameManager.Instance.CurrentScoreToBeat.ToString();

        RefreshShop();
        RefreshHand();
    }

    private void RefreshHand()
    {
        var handHost = rootElement.Q<VisualElement>("_HandHost");

        // Are we in the "playing" state and haven't yet chosen a hand?
        if (
            GameManager.Instance.TryGetState<PlayingState>(out var playingState)
            && !playingState.HasChosenLayout
        )
        {
            // Make shop visible (using translation to animate show/hide)
            handHost.style.translate = new StyleTranslate(new Translate(0, 0));
        }
        else
        {
            // Make shop hidden
            handHost.style.translate = new StyleTranslate(new Translate(0, 340));
        }
    }

    private void RefreshShop()
    {
        var shopHost = rootElement.Q<VisualElement>("_ShopHost");
        var shopPackHost = rootElement.Q<VisualElement>("_ShopPackHost");

        // Are we in the "shop" state?
        if (GameManager.Instance.TryGetState<ShopState>(out var shopState))
        {
            // Make shop visible (using translation to animate show/hide)
            shopHost.style.translate = new StyleTranslate(new Translate(0, 0));
            shopPackHost.style.translate = new StyleTranslate(
                new Translate(0, shopState.IsOpeningPack ? 0 : 340)
            );

            RefreshShopPack();
        }
        else
        {
            // Make shop hidden
            shopHost.style.translate = new StyleTranslate(new Translate(0, -340));
            shopPackHost.style.translate = new StyleTranslate(new Translate(0, 340));
        }
    }

    private void RefreshShopPack()
    {
        var shopManager = GameManager.Instance.ShopManager;

        // Hide and early out if no pack is available.
        if (shopManager.CurrentPack == null)
        {
            rootElement.Q<VisualElement>("_Shop_ItemPack").style.visibility = Visibility.Hidden;
            return;
        }
        else
        {
            rootElement.Q<VisualElement>("_Shop_ItemPack").style.visibility = Visibility.Visible;
        }

        // Update pack text
        var packButton = rootElement.Q<Button>("_Shop_ItemPackButton");
        packButton.text =
            $"{shopManager.CurrentPack.PackName}\n(${shopManager.CurrentPack.PackCost})";

        // Add pack cards to UI (every refresh for now)
        var cardsContainer = rootElement.Q<VisualElement>("_ShopPack_CardsContainer");
        cardsContainer.Clear();
        foreach (var packCard in shopManager.CurrentPack.PackCards)
        {
            var element = new Button();
            element.AddToClassList("shop-pack-card");
            element.text = packCard.name;

            // Card click handler
            element.clicked += () =>
            {
                // Request hiding the pack UI
                (GameManager.Instance.CurrentState as ShopState).IsOpeningPack = false;

                // Claim this pack
                GameManager.Instance.ShopManager.ClaimPackCard(packCard);
            };

            cardsContainer.Add(element);
        }
    }
}
