using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class WinUI : MonoBehaviour
{
    [Header("General")]
    [SerializeField] private GameAssetSO gameAssetSO;
    [SerializeField] private CanvasGroup popup;
    [SerializeField] private TMP_Text coinCollectedText;
    [SerializeField] private Image coinImage;
    [SerializeField] private Image coinOutline;
    [SerializeField] private Button continueButton;
    [SerializeField] private Button collectDoubleButton;

    [Header("Gold Coin")]
    [SerializeField] private Color goldCoinOutlineColor;
    [SerializeField] private Color goldCoinButtonColor;

    [Header("Azure Coin")]
    [SerializeField] private Color azureCoinOutlineColor;
    [SerializeField] private Color azureCoinButtonColor;


    public void Show(CurrencyType currencyType, int coinCollectedAmount, Action onContinueButtonClicked)
    {
        AudioManager.Instance.PlayCoinFeedbacks();

        // popup.alpha = 0;
        popup.gameObject.SetActive(true);
        popup.DOFade(1, 0.1f);

        coinCollectedText.text = "+" + coinCollectedAmount;

        collectDoubleButton.onClick.AddListener(() =>
        {
            Debug.Log("Clicked");

            AudioManager.Instance.PlayClickFeedbacks();

            MonetizationManager.Instance.ShowRewardedVideo(() =>
            {
                GameDataManager.Instance.SetCoinCollectedDouble(currencyType);
                onContinueButtonClicked?.Invoke();
            });
        });

        if (currencyType == CurrencyType.GoldCoin)
        {
            coinImage.sprite = gameAssetSO.GoldCoinSprite;
            coinOutline.color = goldCoinOutlineColor;

            collectDoubleButton.GetComponent<Image>().color = azureCoinButtonColor;
        }
        else
        {
            coinImage.sprite = gameAssetSO.AzureCoinSprite;
            coinOutline.color = azureCoinOutlineColor;

            collectDoubleButton.GetComponent<Image>().color = goldCoinButtonColor;
        }

        continueButton.onClick.RemoveAllListeners();
        continueButton.onClick.AddListener(() =>
        {
            AudioManager.Instance.PlayClickFeedbacks();
            onContinueButtonClicked?.Invoke();
        });
    }

    public void Hide()
    {
        // popup.alpha = 1;
        popup.DOFade(0, 0.1f).OnComplete(() =>
        {
            popup.gameObject.SetActive(false);
        });
    }
}
