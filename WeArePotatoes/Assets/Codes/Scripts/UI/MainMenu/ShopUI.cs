using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Farou.Utility;
using Sirenix.OdinInspector;
using MoreMountains.Feedbacks;
using MoreMountains.Tools;
using DG.Tweening;

/// <summary>
/// Handles the Shop UI interactions such as card purchases and currency transactions.
/// </summary>
public class ShopUI : MonoBehaviour
{
    [Header("General")]
    [Tooltip("CanvasGroup used to fade in/out the shop panel.")]
    [SerializeField] private CanvasGroup panel;
    [Tooltip("Color to indicate the active button state.")]
    [SerializeField] private Color activeColor;
    [Tooltip("Color to indicate the non-active button state.")]
    [SerializeField] private Color nonActiveColor;

    [Header("Card UI")]
    [SerializeField] private ShopDatabaseSO shopDatabaseSO;
    [Tooltip("Script that manages the card reveal animations.")]
    [SerializeField] private CardRevealUI cardRevealUI;
    [Tooltip("Button to switch to card view.")]
    [SerializeField] private Button cardButton;
    [Tooltip("Transform containing the card layout UI.")]
    [SerializeField] private Transform cardLayoutTransform;
    [Tooltip("Button to buy one card.")]
    [SerializeField] private Button oneCardButton;
    [Tooltip("Button to buy ten cards.")]
    [SerializeField] private Button tenCardsButton;
    [SerializeField] private TMP_Text oneCardPriceText;
    [SerializeField] private TMP_Text tenCardsPriceText;

    [Header("Currency UI")]
    [Tooltip("Button to switch to currency view.")]
    [SerializeField] private Button currencyButton;
    [Tooltip("Transform containing the currency layout UI.")]
    [SerializeField] private Transform currencyLayoutTransform;
    [Tooltip("Button to purchase a small amount of currency.")]
    [SerializeField] private Button smallCurrencyButton;
    [Tooltip("Button to purchase a medium amount of currency.")]
    [SerializeField] private Button mediumCurrencyButton;
    [Tooltip("Button to purchase a large amount of currency.")]
    [SerializeField] private Button bigCurrencyButton;
    [Tooltip("Button to watch ads and receive currency.")]
    [SerializeField] private Button watchAdsCurrencyButton;

    private void Awake()
    {
        // Card UI
        cardButton.onClick.AddListener(OnCardButtonClicked);
        oneCardButton.onClick.AddListener(BuyOneCard);
        tenCardsButton.onClick.AddListener(BuyTenCard);

        // Currency UI
        currencyButton.onClick.AddListener(OnCurrencyButtonClicked);
        smallCurrencyButton.onClick.AddListener(PurchaseSmallCurrency);
        mediumCurrencyButton.onClick.AddListener(PurchaseMediumCurrency);
        bigCurrencyButton.onClick.AddListener(PurchaseBigCurrency);
        watchAdsCurrencyButton.onClick.AddListener(PurchaseWatchAdsCurrency);
    }

    private void Start()
    {
        // Initialize UI to card view
        SetActiveLayout(cardLayoutTransform, cardButton, currencyLayoutTransform, currencyButton);
        panel.gameObject.SetActive(false); // Hide panel by default

        oneCardPriceText.text = shopDatabaseSO.OneCardPrice.ToString();
        tenCardsPriceText.text = shopDatabaseSO.TenCardsPrice.ToString();
    }

    /// <summary>
    /// Called when the card button is clicked to switch to the card view.
    /// </summary>
    private void OnCardButtonClicked()
    {
        AudioManager.Instance.PlayClickFeedbacks();
        SetActiveLayout(cardLayoutTransform, cardButton, currencyLayoutTransform, currencyButton);
    }

    /// <summary>
    /// Called when the currency button is clicked to switch to the currency view.
    /// </summary>
    private void OnCurrencyButtonClicked()
    {
        AudioManager.Instance.PlayClickFeedbacks();
        SetActiveLayout(currencyLayoutTransform, currencyButton, cardLayoutTransform, cardButton);
    }

    /// <summary>
    /// Purchases one card and initiates the card reveal UI.
    /// </summary>
    private void BuyOneCard()
    {
        AudioManager.Instance.PlayClickFeedbacks();

        var gameDataManager = GameDataManager.Instance;

        if (gameDataManager.AzureCoin >= shopDatabaseSO.OneCardPrice)
        {
            cardRevealUI.Show();
            StartCoroutine(cardRevealUI.InitializeOneCard());
            gameDataManager.ModifyAzureCoin(-shopDatabaseSO.OneCardPrice);
        }
    }

    /// <summary>
    /// Purchases ten cards and initiates the card reveal UI.
    /// </summary>
    private void BuyTenCard()
    {
        AudioManager.Instance.PlayClickFeedbacks();

        var gameDataManager = GameDataManager.Instance;

        if (gameDataManager.AzureCoin >= shopDatabaseSO.TenCardsPrice)
        {
            cardRevealUI.Show();
            StartCoroutine(cardRevealUI.InitializeTenCards());
            gameDataManager.ModifyAzureCoin(-shopDatabaseSO.TenCardsPrice);
        }
    }

    /// <summary>
    /// Purchases a small amount of currency.
    /// </summary>
    private void PurchaseSmallCurrency()
    {
        AudioManager.Instance.PlayClickFeedbacks();
        // Handle the purchase logic for small currency here
    }

    /// <summary>
    /// Purchases a medium amount of currency.
    /// </summary>
    private void PurchaseMediumCurrency()
    {
        AudioManager.Instance.PlayClickFeedbacks();
        // Handle the purchase logic for medium currency here
    }

    /// <summary>
    /// Purchases a large amount of currency.
    /// </summary>
    private void PurchaseBigCurrency()
    {
        AudioManager.Instance.PlayClickFeedbacks();
        // Handle the purchase logic for big currency here
    }

    /// <summary>
    /// Initiates the process to watch ads for currency rewards.
    /// </summary>
    private void PurchaseWatchAdsCurrency()
    {
        AudioManager.Instance.PlayClickFeedbacks();
        // Handle the logic for watching ads and giving rewards here
    }

    /// <summary>
    /// Displays the shop UI by fading in the panel.
    /// </summary>
    public void Show()
    {
        panel.gameObject.SetActive(true);
        panel.alpha = 0;
        panel.DOFade(1, 0.1f); // Fade in effect
    }

    /// <summary>
    /// Hides the shop UI by fading out the panel.
    /// </summary>
    public void Hide()
    {
        panel.alpha = 1;
        panel.DOFade(0, 0.1f).OnComplete(() =>
        {
            panel.gameObject.SetActive(false);
        });
    }

    /// <summary>
    /// Sets the active layout (card or currency) and updates button colors accordingly.
    /// </summary>
    /// <param name="activeLayout">The transform of the active layout.</param>
    /// <param name="activeButton">The button representing the active layout.</param>
    /// <param name="inactiveLayout">The transform of the inactive layout.</param>
    /// <param name="inactiveButton">The button representing the inactive layout.</param>
    private void SetActiveLayout(Transform activeLayout, Button activeButton, Transform inactiveLayout, Button inactiveButton)
    {
        activeLayout.gameObject.SetActive(true);
        inactiveLayout.gameObject.SetActive(false);

        activeButton.GetComponent<Image>().color = activeColor;
        inactiveButton.GetComponent<Image>().color = nonActiveColor;
    }

    private void OnDestroy()
    {
        // Unsubscribe from button events to prevent memory leaks
        cardButton.onClick.RemoveListener(OnCardButtonClicked);
        oneCardButton.onClick.RemoveListener(BuyOneCard);
        tenCardsButton.onClick.RemoveListener(BuyTenCard);

        currencyButton.onClick.RemoveListener(OnCurrencyButtonClicked);
        smallCurrencyButton.onClick.RemoveListener(PurchaseSmallCurrency);
        mediumCurrencyButton.onClick.RemoveListener(PurchaseMediumCurrency);
        bigCurrencyButton.onClick.RemoveListener(PurchaseBigCurrency);
        watchAdsCurrencyButton.onClick.RemoveListener(PurchaseWatchAdsCurrency);
    }
}
