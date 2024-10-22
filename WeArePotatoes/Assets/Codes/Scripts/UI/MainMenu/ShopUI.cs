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

public class ShopUI : MonoBehaviour
{
    [Header("General")]
    [SerializeField] private CanvasGroup panel;
    [SerializeField] private Color activeColor;
    [SerializeField] private Color nonActiveColor;

    [Header("Card UI")]
    [SerializeField] private Button cardButton;
    [SerializeField] private Transform cardLayoutTransform;
    [SerializeField] private Button oneCardButton;
    [SerializeField] private Button tenCardsButton;

    [Header("Currency UI")]
    [SerializeField] private Button currencyButton;
    [SerializeField] private Transform currencyLayoutTransform;
    [SerializeField] private Button smallCurrencyButton;
    [SerializeField] private Button mediumCurrencysButton;
    [SerializeField] private Button bigCurrencysButton;
    [SerializeField] private Button watchAdsCurrencysButton;

    private void Awake()
    {
        // Card UI
        cardButton.onClick.AddListener(OnCardButtonClicked);
        oneCardButton.onClick.AddListener(BuyOneCard);
        tenCardsButton.onClick.AddListener(BuyTenCard);

        // Currency UI
        currencyButton.onClick.AddListener(OnCurrencyButtonClicked);
        smallCurrencyButton.onClick.AddListener(PurchaseSmallCurrency);
        mediumCurrencysButton.onClick.AddListener(PurchaseMediumCurrency);
        bigCurrencysButton.onClick.AddListener(PurchaseBigCurrency);
        watchAdsCurrencysButton.onClick.AddListener(PurchaseWatchAdsCurrency);
    }

    private void Start()
    {
        cardLayoutTransform.gameObject.SetActive(true);
        currencyLayoutTransform.gameObject.SetActive(false);
        cardButton.GetComponent<Image>().color = activeColor;
        currencyButton.GetComponent<Image>().color = nonActiveColor;

        panel.gameObject.SetActive(false);
    }

    private void OnCardButtonClicked()
    {
        AudioManager.Instance.PlayClickFeedbacks();
        cardLayoutTransform.gameObject.SetActive(true);
        currencyLayoutTransform.gameObject.SetActive(false);
        cardButton.GetComponent<Image>().color = activeColor;
        currencyButton.GetComponent<Image>().color = nonActiveColor;
    }

    private void OnCurrencyButtonClicked()
    {
        AudioManager.Instance.PlayClickFeedbacks();
        cardLayoutTransform.gameObject.SetActive(false);
        currencyLayoutTransform.gameObject.SetActive(true);
        cardButton.GetComponent<Image>().color = nonActiveColor;
        currencyButton.GetComponent<Image>().color = activeColor;
    }

    private void BuyOneCard()
    {
        AudioManager.Instance.PlayClickFeedbacks();
    }

    private void BuyTenCard()
    {
        AudioManager.Instance.PlayClickFeedbacks();
    }

    private void PurchaseSmallCurrency()
    {
        AudioManager.Instance.PlayClickFeedbacks();
    }

    private void PurchaseMediumCurrency()
    {
        AudioManager.Instance.PlayClickFeedbacks();
    }

    private void PurchaseBigCurrency()
    {
        AudioManager.Instance.PlayClickFeedbacks();
    }

    private void PurchaseWatchAdsCurrency()
    {
        AudioManager.Instance.PlayClickFeedbacks();
    }

    public void Show()
    {
        panel.gameObject.SetActive(true);
        panel.alpha = 0;
        panel.DOFade(1, 0.1f);
    }

    public void Hide()
    {
        panel.alpha = 1;
        panel.DOFade(0, 0.1f).OnComplete(() =>
        {
            panel.gameObject.SetActive(false);
        });
    }
}
