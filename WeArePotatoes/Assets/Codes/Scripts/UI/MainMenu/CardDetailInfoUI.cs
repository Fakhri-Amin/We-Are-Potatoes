using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using DG.Tweening;
using Unity.VisualScripting;

public class CardDetailInfoUI : MonoBehaviour
{
    [Header("General")]
    [SerializeField] private CanvasGroup panel;
    [SerializeField] private Button closeButton;
    [SerializeField] private Image cardIcon;
    [SerializeField] private TMP_Text cardName;
    [SerializeField] private TMP_Text cardLevel;
    [SerializeField] private TMP_Text cardAmount;
    [SerializeField] private TMP_Text cardDescription;
    [SerializeField] private TMP_Text cardEffectDescription;
    [SerializeField] private Slider cardAmountSlider;
    [SerializeField] private Image cardSliderFillImage;
    [SerializeField] private Color cardSliderFillNormalColor;
    [SerializeField] private Color cardSliderFillUpgradeColor;

    [Header("Upgrade Button")]
    [SerializeField] private Button upgradeButton;
    [SerializeField] private Image upgradeIcon;

    private void Awake()
    {
        closeButton.onClick.AddListener(Hide);
    }

    private void Start()
    {
        panel.gameObject.SetActive(false);
    }

    public void Select(CardUI cardUI, ObtainedCard obtainedCard, CardLevelConfig cardLevelConfig, Sprite cardSprite)
    {
        Show();

        cardIcon.sprite = cardSprite;
        cardName.text = obtainedCard.CardData.Name;
        cardLevel.text = "Level " + obtainedCard.Level;
        cardAmount.text = obtainedCard.CardAmount + "/" + cardLevelConfig.MaxCardAmount;
        cardDescription.text = obtainedCard.CardData.Description;
        cardEffectDescription.text = obtainedCard.CardData.EffectDescription;

        cardAmountSlider.minValue = 0;
        cardAmountSlider.maxValue = cardLevelConfig.MaxCardAmount;
        cardAmountSlider.value = obtainedCard.CardAmount;

        upgradeButton.onClick.RemoveAllListeners();
        upgradeButton.onClick.AddListener(() =>
        {
            AudioManager.Instance.PlayClickFeedbacks();
            GameDataManager.Instance.UpgradeObtainedCard(obtainedCard);
            cardUI.Initialize();
            Hide();
        });

        if (obtainedCard.CardAmount >= cardLevelConfig.MaxCardAmount)
        {
            upgradeButton.interactable = true;
            cardSliderFillImage.color = cardSliderFillUpgradeColor;
            upgradeIcon.gameObject.SetActive(true);
        }
        else
        {
            upgradeButton.interactable = false;
            cardSliderFillImage.color = cardSliderFillNormalColor;
            upgradeIcon.gameObject.SetActive(false);
        }
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

        AudioManager.Instance.PlayClickFeedbacks();
    }
}
