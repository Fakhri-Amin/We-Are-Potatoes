using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Linq;
using DG.Tweening;

public class CardRevealSlotUI : MonoBehaviour
{
    [SerializeField] private CanvasGroup panel;
    [SerializeField] private Image cardIcon;
    [SerializeField] private TMP_Text cardLevelText;

    private void Start()
    {
        panel.alpha = 0;
    }

    public void Initialize(ObtainedCard obtainedCard, Sprite cardSprite)
    {
        cardIcon.sprite = cardSprite;
        cardLevelText.text = "Level " + obtainedCard.Level;
    }

    public void Show()
    {
        AudioManager.Instance.PlayClickFeedbacks();
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
