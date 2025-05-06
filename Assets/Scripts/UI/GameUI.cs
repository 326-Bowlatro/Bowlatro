using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using static GameManager;

public partial class GameUI : MonoBehaviour
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
            GameManager.Instance.GoToState<PreRoundState>();
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
        rootElement.Q<Button>("_ShopPack_Cancel").clicked += () =>
        {
            (GameManager.Instance.CurrentState as ShopState).IsOpeningPack = false;
            Refresh();
        };
        rootElement.Q<CardElement>("_Shop_ItemPack").OnClick += () =>
        {
            (GameManager.Instance.CurrentState as ShopState).IsOpeningPack = true;
            Refresh();
        };
        rootElement.Q<Button>("_Hand_PlayHand").clicked += () =>
        {
            GameManager.Instance.GoToState<PlayingState>();
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
        RefreshTickets();
    }

    private void RefreshTickets()
    {
        var inventoryManager = GameManager.Instance.InventoryManager;

        // Only hide tickets panel in "playing" and "results" states (or if we have no tickets)
        var shouldHideTickets =
            GameManager.Instance.CurrentState is PlayingState
            || GameManager.Instance.CurrentState is ResultsState
            || inventoryManager.CurrentTickets.Count == 0;

        // Only hide tickets panel in "playing" state
        var ticketsHost = rootElement.Q<VisualElement>("_TicketsHost");
        ticketsHost.style.translate = new StyleTranslate(
            new Translate(shouldHideTickets ? 264 : 0, 0)
        );

        // Add tickets to UI (every refresh for now)
        var ticketsContainer = rootElement.Q<VisualElement>("_Tickets_TicketsContainer");
        ticketsContainer.Clear();
        foreach (var ticket in inventoryManager.CurrentTickets)
        {
            var element = new TicketElement(ticket);
            ticketsContainer.Add(element);
        }

        // Update ticket count for display
        rootElement.Q<Label>("_Tickets_TicketCount").text =
            $"{inventoryManager.CurrentTickets.Count}/{InventoryManager.MaxTickets}";
    }

    private void RefreshHand()
    {
        var handHost = rootElement.Q<VisualElement>("_HandHost");

        // Are we in the "pre-round" state?
        if (GameManager.Instance.CurrentState is PreRoundState)
        {
            // Make hand visible (using translation to animate show/hide)
            handHost.style.translate = new StyleTranslate(new Translate(0, 0));

            // Set button status based on current state
            rootElement
                .Q<Button>("_Hand_PlayHand")
                .SetEnabled(GameManager.Instance.SelectedLayout != null);

            RefreshHandCards();
        }
        else
        {
            // Make hand hidden
            handHost.style.translate = new StyleTranslate(new Translate(0, 502));
        }
    }

    private void RefreshHandCards()
    {
        var shopManager = GameManager.Instance.ShopManager;
        var inventoryManager = GameManager.Instance.InventoryManager;

        // Update layout count for display
        rootElement.Q<Label>("_Hand_LayoutCount").text =
            $"{inventoryManager.CurrentDeck.OfType<PinLayoutCard>().Count()}/{InventoryManager.MaxLayouts}";

        // Update booster count for display
        rootElement.Q<Label>("_Hand_BoosterCount").text =
            $"{inventoryManager.CurrentDeck.OfType<BoosterCard>().Count()}/{InventoryManager.MaxBoosters}";

        // Add layout cards to UI (every refresh for now)
        var layoutsContainer = rootElement.Q<VisualElement>("_Hand_LayoutsContainer");
        layoutsContainer.Clear();
        foreach (var card in inventoryManager.CurrentHandLayouts)
        {
            // Skip if the card is in the layout slot (pretend it's been "moved")
            if (card == GameManager.Instance.SelectedLayout)
            {
                continue;
            }

            var element = new CardElement();
            element.SetCard(card);

            // Card click handler (set as current layout)
            element.OnClick += () =>
            {
                GameManager.Instance.SelectedLayout = card;
                Refresh();
            };

            layoutsContainer.Add(element);
        }

        // Add boster cards to UI (every refresh for now)
        var boostersContainer = rootElement.Q<VisualElement>("_Hand_BoostersContainer");
        boostersContainer.Clear();
        foreach (var card in inventoryManager.CurrentHandBoosters)
        {
            var element = new CardElement();
            element.SetCard(card);

            // TODO: Card click handler (use booster)
            // element.OnClick += () =>
            // {
            //     GameManager.Instance.SelectedLayout = card;
            //     Refresh();
            // };

            boostersContainer.Add(element);
        }

        // Add selected layout card to UI
        var layoutContainer = rootElement.Q<VisualElement>("_Hand_LayoutContainer");
        layoutContainer.Clear();
        if (GameManager.Instance.TryGetState<PreRoundState>(out var state))
        {
            // Layout can be null here (if slot is empty). That's fine, just hide it.
            var layout = GameManager.Instance.SelectedLayout;
            var element = new CardElement();
            element.SetCard(layout);

            // Card click handler (clear current layout)
            element.OnClick += () =>
            {
                GameManager.Instance.SelectedLayout = null;
                Refresh();
            };

            layoutContainer.Add(element);
        }
    }

    private void RefreshShop()
    {
        var shopHost = rootElement.Q<VisualElement>("_ShopHost");
        var shopPackHost = rootElement.Q<VisualElement>("_ShopPackHost");

        // Are we in the "shop" state?
        if (GameManager.Instance.TryGetState<ShopState>(out var state))
        {
            // Make shop visible (using translation to animate show/hide)
            shopHost.style.translate = new StyleTranslate(new Translate(0, 0));
            shopPackHost.style.translate = new StyleTranslate(
                new Translate(0, state.IsOpeningPack ? 0 : 340)
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
        var shopPack = rootElement.Q<CardElement>("_Shop_ItemPack");
        shopPack.IsHidden = shopManager.CurrentPack == null;

        // Hide and early out if no pack is available.
        if (shopManager.CurrentPack == null)
        {
            return;
        }

        // Update pack info
        shopPack.SetCard(
            $"${shopManager.CurrentPack.PackCost} Pack",
            "Choose 1 of up to 3 LAYOUT cards to add to your deck",
            null
        );

        // Add pack cards to UI (every refresh for now)
        var cardsContainer = rootElement.Q<VisualElement>("_ShopPack_CardsContainer");
        cardsContainer.Clear();
        foreach (var packCard in shopManager.CurrentPack.PackCards)
        {
            var element = new CardElement();
            element.SetCard(packCard);

            // Card click handler
            element.OnClick += () =>
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
