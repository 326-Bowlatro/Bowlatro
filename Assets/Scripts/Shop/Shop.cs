using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Shop : MonoBehaviour
{
    public ItemPack[] packs;

    // Our UI reference to the packs
    public PackUI[] packUIs;

    public Button buyButton;
    public TextMeshProUGUI tooltipText;

    [Header("Pin Selection")] public List<GameObject> specialPinPrefabs;
    public List<GameObject> pinPrefabs; 
    public Transform[] spawnPoints; 
    public float pinCost = 50f; 

    [Header("UI")]
    public Button purchaseButton;

    private ItemPack selectedPack;
    private PackUI selectedPackUI;
    private GameObject selectedPinPrefab;
    private List<GameObject> spawnedPins = new List<GameObject>();
    private GameObject selectedPin;



    void Awake()
    {
        //GameObject.Find("ShopOverlay").SetActive(false);
    }

    void Start()
    {
        SetupShop();
//        buyButton.interactable = false;
//        tooltipText.gameObject.SetActive(false);
//        buyButton.onClick.AddListener(BuySelectedPack);

        SpawnPins();
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
    void SpawnPins()
    {
        // Clear old pins
        foreach (var pin in spawnedPins)
        {
            if (pin != null) Destroy(pin);
        }
        spawnedPins.Clear();

        // Spawn available pins
        for (int i = 0; i < Mathf.Min(pinPrefabs.Count, spawnPoints.Length); i++)
        {
            GameObject pin = Instantiate(
                pinPrefabs[i], 
                spawnPoints[i].position,
                spawnPoints[i].rotation
            );

            var clickable = pin.AddComponent<ClickablePin>();
            clickable.Initialize(this);
            
            spawnedPins.Add(pin);
        }
    }

    public void SelectPin(GameObject pin)
    {
        selectedPin = pin;
    }

    public void ConfirmSelection()
    {
        if (selectedPin == null) return;

        // Get the base prefab (not the scene instance)
        GameObject pinPrefab = selectedPin.GetComponent<ShopPin>().originalPrefab;
        
        // Add to special pins
        PinLayoutManager.EnableSpecialPins(
            enable: true,
            prefabs: new List<GameObject>{ pinPrefab },
            count: 1
        );

        Debug.Log($"Added {pinPrefab.name} to next round");
    }
}
