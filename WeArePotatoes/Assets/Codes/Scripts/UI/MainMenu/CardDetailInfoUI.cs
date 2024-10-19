using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using DG.Tweening;

public class CardDetailInfoUI : MonoBehaviour
{
    [SerializeField] private CanvasGroup panel;
    [SerializeField] private Button closeButton;
    [SerializeField] private Image cardIcon;
    [SerializeField] private TMP_Text cardName;
    [SerializeField] private TMP_Text cardLevel;
    [SerializeField] private TMP_Text cardAmount;
    [SerializeField] private TMP_Text cardDescription;
    [SerializeField] private TMP_Text cardEffectDescription;
    [SerializeField] private Slider cardAmountSlider;

    private void Awake()
    {
        closeButton.onClick.AddListener(Hide);
    }

    private void Start()
    {
        panel.gameObject.SetActive(false);
    }

    public void Select(ObtainedCard obtainedCard, CardLevelConfig cardLevelConfig)
    {
        Show();

        cardIcon.sprite = obtainedCard.CardData.Sprite;
        cardName.text = obtainedCard.CardData.Name;
        cardLevel.text = "Level " + obtainedCard.Level;
        cardAmount.text = obtainedCard.CardAmount + "/" + cardLevelConfig.MaxCardAmount;
        cardDescription.text = obtainedCard.CardData.Description;
        cardEffectDescription.text = obtainedCard.CardData.EffectDescription;

        cardAmountSlider.minValue = 0;
        cardAmountSlider.maxValue = cardLevelConfig.MaxCardAmount;
        cardAmountSlider.value = obtainedCard.CardAmount;
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
