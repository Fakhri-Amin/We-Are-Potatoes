using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using DG.Tweening;

public class CardUI : MonoBehaviour
{
    [SerializeField] private CardDatabaseSO cardDatabaseSO;
    [SerializeField] private CanvasGroup panel;
    [SerializeField] private CardSlotUI cardSlotTemplate;
    [SerializeField] private Transform cardParent;
    [SerializeField] private CardDetailInfoUI cardDetailInfoUI;

    private void Start()
    {
        cardSlotTemplate.gameObject.SetActive(false);

        panel.gameObject.SetActive(false);
    }

    public void Initialize()
    {
        foreach (Transform child in cardParent)
        {
            if (child.GetComponent<CardSlotUI>() == cardSlotTemplate) continue;
            Destroy(child.gameObject);
        }

        GameDataManager gameDataManager = GameDataManager.Instance;

        foreach (var item in gameDataManager.ObtainedCardList)
        {
            CardSlotUI cardSlotUI = Instantiate(cardSlotTemplate, cardParent);
            cardSlotUI.gameObject.SetActive(true);

            Sprite cardSprite = cardDatabaseSO.CardDatas.Find(i => i.Name == item.CardData.Name).Sprite;

            CardLevelConfig cardLevelConfig = cardDatabaseSO.CardLevelConfigList.Find(i => i.Level == item.Level);

            cardSlotUI.Initialize(item, cardLevelConfig, cardSprite, () =>
            {
                cardDetailInfoUI.Select(this, item, cardLevelConfig, cardSprite);
                AudioManager.Instance.PlayClickFeedbacks();
            });
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

    [Button]
    public void AddAttackDamageCard()
    {
        CardData cardData = cardDatabaseSO.CardDatas[0];

        GameDataManager.Instance.AddNewObtainedCard(cardData);
        Initialize();
    }

    [Button]
    public void AddUnitHealthCard()
    {
        CardData cardData = cardDatabaseSO.CardDatas[1];

        GameDataManager.Instance.AddNewObtainedCard(cardData);
        Initialize();
    }

    [Button]
    public void AddBaseHealthCard()
    {
        CardData cardData = cardDatabaseSO.CardDatas[2];

        GameDataManager.Instance.AddNewObtainedCard(cardData);
        Initialize();
    }

    [Button]
    public void AddAllCard()
    {
        foreach (var item in cardDatabaseSO.CardDatas)
        {
            GameDataManager.Instance.AddNewObtainedCard(item);
            Initialize();
        }
    }
}
