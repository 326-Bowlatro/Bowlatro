using System;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    public ItemPack[] packs;
    // Our UI reference to the packs
    public PackUI[] packUIs;
    
    public Button buyButton;
    public TextMeshProUGUI tooltipText;

    private ItemPack selectedPack;
    private PackUI selectedPackUI;

    void Awake()
    {
        GameObject.Find("ShopOverlay").SetActive(false);
    }

    void Start()
    {
        SetupShop();
        buyButton.interactable = false;
        tooltipText.gameObject.SetActive(false);
        buyButton.onClick.AddListener(BuySelectedPack);
    }

    // Will assign the data from our packs to the UI
    void SetupShop()
    {
        for (int i = 0; i < packs.Length; i++)
        {
            packUIs[i].SetItem(packs[i]);
            packUIs[i].OnClick(ui => SelectPack(ui));
            
            // Show the description of the item
            packUIs[i].OnHover(desc => ShowTooltip(desc));
            // Hide the tooltip after moving mouse away
            packUIs[i].OnExitHover(HideTooltip);
        }
    }

    // When a specific pack is clicked
    void SelectPack(PackUI ui)
    {
        // return if we already bought the pack
        if (ui.Item.isBought) return; 

        selectedPack = ui.Item;
        selectedPackUI = ui;

        // Ensure all other packs are unselected when selecting a pack
        foreach (var pack in packUIs)
            pack.Deselect();

        ui.Select();
        buyButton.interactable = true;
    }

    // When we click buy on the pack
    void BuySelectedPack()
    {
        if (selectedPack == null || selectedPack.isBought) return;

        Debug.Log($"Bought a {selectedPack.packName} for: ${selectedPack.price}");
        
        selectedPack.isBought = true;
        selectedPackUI.MarkAsBought(); 

        buyButton.interactable = false; 
    }

    // Shows the description when hovering over a pack
    void ShowTooltip(string text)
    {
        tooltipText.text = text;
        tooltipText.gameObject.SetActive(true);
    }

    // Hides the tooltip when mouse leaves
    void HideTooltip()
    {
        tooltipText.gameObject.SetActive(false);
    }
}