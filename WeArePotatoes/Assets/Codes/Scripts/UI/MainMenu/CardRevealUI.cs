using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Linq;
using DG.Tweening;
using Unity.Mathematics;
using Unity.VisualScripting;

public class CardRevealUI : MonoBehaviour
{
    [SerializeField] private CardDatabaseSO cardDatabaseSO;
    [SerializeField] private CanvasGroup panel;
    [SerializeField] private Transform cardParent;
    [SerializeField] private CardRevealSlotUI cardRevealSlotTemplate;

    private void Awake()
    {
        panel.GetComponent<Button>().onClick.AddListener(() =>
        {
            Hide();
        });
    }

    private void Start()
    {
        cardRevealSlotTemplate.gameObject.SetActive(false);

        panel.gameObject.SetActive(false);
    }

    public void InitializeOneCard()
    {
        foreach (Transform child in cardParent)
        {
            if (child.GetComponent<CardRevealSlotUI>() == cardRevealSlotTemplate) continue;
            Destroy(child.gameObject);
        }

        int cardCount = 1;

        for (int i = 0; i < cardCount; i++)
        {
            int randomNumber = UnityEngine.Random.Range(0, cardDatabaseSO.CardDatas.Count);
            CardData cardData = cardDatabaseSO.CardDatas[randomNumber];

            GameDataManager.Instance.AddNewObtainedCard(cardData);

            CardRevealSlotUI cardRevealSlotUI = Instantiate(cardRevealSlotTemplate, cardParent);

            GameDataManager.Instance.TryGetObtainedCard(cardData, out ObtainedCard obtainedCard);

            CardLevelConfig cardLevelConfig = cardDatabaseSO.CardLevelConfigList.Find(i => i.Level == obtainedCard.Level);

            cardRevealSlotUI.gameObject.SetActive(true);
            cardRevealSlotUI.Initialize(obtainedCard, cardLevelConfig, cardData.Sprite);
        }
    }

    public void InitializeTenCards()
    {
        foreach (Transform child in cardParent)
        {
            if (child.GetComponent<CardRevealSlotUI>() == cardRevealSlotTemplate) continue;
            Destroy(child.gameObject);
        }

        int cardCount = 10;

        for (int i = 0; i < cardCount; i++)
        {
            int randomNumber = UnityEngine.Random.Range(0, cardDatabaseSO.CardDatas.Count);
            CardData cardData = cardDatabaseSO.CardDatas[randomNumber];

            GameDataManager.Instance.AddNewObtainedCard(cardData);

            CardRevealSlotUI cardRevealSlotUI = Instantiate(cardRevealSlotTemplate, cardParent);

            GameDataManager.Instance.TryGetObtainedCard(cardData, out ObtainedCard obtainedCard);

            CardLevelConfig cardLevelConfig = cardDatabaseSO.CardLevelConfigList.Find(i => i.Level == obtainedCard.Level);

            cardRevealSlotUI.gameObject.SetActive(true);
            cardRevealSlotUI.Initialize(obtainedCard, cardLevelConfig, cardData.Sprite);
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
    }
}
