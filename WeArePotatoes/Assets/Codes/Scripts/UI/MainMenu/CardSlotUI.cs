using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Linq;
using DG.Tweening;

public class CardSlotUI : MonoBehaviour
{
    [SerializeField] private Image cardIcon;
    [SerializeField] private TMP_Text cardLevelText;
    [SerializeField] private TMP_Text cardAmountText;
    [SerializeField] private Slider cardAmountSlider;
    [SerializeField] private Image cardSliderFillImage;
    [SerializeField] private Color cardSliderFillNormalColor;
    [SerializeField] private Color cardSliderFillUpgradeColor;
    [SerializeField] private Button button;
    [SerializeField] private Image upgradeIcon;

    public void Initialize(ObtainedCard obtainedCard, CardLevelConfig cardLevelConfig, Sprite cardSprite, Action onButtonClicked)
    {
        cardIcon.sprite = cardSprite;
        cardLevelText.text = "Level " + obtainedCard.Level;
        cardAmountText.text = obtainedCard.CardAmount + "/" + cardLevelConfig.MaxCardAmount;

        cardAmountSlider.minValue = 0;
        cardAmountSlider.maxValue = cardLevelConfig.MaxCardAmount;
        cardAmountSlider.value = obtainedCard.CardAmount;

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => onButtonClicked());

        if (obtainedCard.CardAmount >= cardLevelConfig.MaxCardAmount)
        {
            cardSliderFillImage.color = cardSliderFillUpgradeColor;
            upgradeIcon.gameObject.SetActive(true);
        }
        else
        {
            cardSliderFillImage.color = cardSliderFillNormalColor;
            upgradeIcon.gameObject.SetActive(false);
        }
    }
}
