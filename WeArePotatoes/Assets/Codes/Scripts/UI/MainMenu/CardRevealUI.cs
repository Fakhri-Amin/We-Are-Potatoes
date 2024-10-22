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

/// <summary>
/// Manages the card reveal UI logic, including showing obtained cards and handling reveal animations.
/// </summary>
public class CardRevealUI : MonoBehaviour
{
    [Tooltip("Reference to the Card Database Scriptable Object.")]
    [SerializeField] private CardDatabaseSO cardDatabaseSO;

    [Tooltip("CanvasGroup used to fade in/out the card reveal panel.")]
    [SerializeField] private CanvasGroup panel;

    [Tooltip("Parent transform where card slots will be instantiated.")]
    [SerializeField] private Transform cardParent;

    [Tooltip("Template for the card reveal slot UI.")]
    [SerializeField] private CardRevealSlotUI cardRevealSlotTemplate;

    private List<CardRevealSlotUI> cardRevealSlotUIs = new List<CardRevealSlotUI>();

    private void Awake()
    {
        // Ensure panel hides when clicking on it
        panel.GetComponent<Button>().onClick.AddListener(Hide);
    }

    private void Start()
    {
        // Initialize UI state
        cardRevealSlotTemplate.gameObject.SetActive(false);
        panel.gameObject.SetActive(false);
    }

    /// <summary>
    /// Initializes the reveal process for one card.
    /// </summary>
    public IEnumerator InitializeOneCard()
    {
        return InitializeCards(1);
    }

    /// <summary>
    /// Initializes the reveal process for ten cards.
    /// </summary>
    public IEnumerator InitializeTenCards()
    {
        return InitializeCards(10);
    }

    /// <summary>
    /// Initializes the card reveal slots for a given card count and displays the cards one by one.
    /// </summary>
    /// <param name="cardCount">The number of cards to reveal.</param>
    private IEnumerator InitializeCards(int cardCount)
    {
        // Clear existing cards and reset the UI
        cardRevealSlotUIs.Clear();
        ClearCardParent();

        // Instantiate new cards based on the card count
        for (int i = 0; i < cardCount; i++)
        {
            CardData cardData = GetRandomCardData();
            GameDataManager.Instance.AddNewObtainedCard(cardData);

            CardRevealSlotUI cardRevealSlotUI = Instantiate(cardRevealSlotTemplate, cardParent);
            GameDataManager.Instance.TryGetObtainedCard(cardData, out ObtainedCard obtainedCard);

            cardRevealSlotUI.gameObject.SetActive(true);
            cardRevealSlotUI.Initialize(obtainedCard, cardData.Sprite);

            cardRevealSlotUIs.Add(cardRevealSlotUI);
        }

        // Show each card one by one with a delay
        foreach (var slotUI in cardRevealSlotUIs)
        {
            slotUI.Show();
            yield return new WaitForSeconds(0.1f);
        }
    }

    /// <summary>
    /// Shows the card reveal UI with a fade-in effect.
    /// </summary>
    public void Show()
    {
        panel.gameObject.SetActive(true);
        panel.alpha = 0;
        panel.DOFade(1, 0.1f); // Fade in effect
    }

    /// <summary>
    /// Hides the card reveal UI with a fade-out effect and triggers a click feedback.
    /// </summary>
    public void Hide()
    {
        AudioManager.Instance.PlayClickFeedbacks();

        panel.alpha = 1;
        panel.DOFade(0, 0.1f).OnComplete(() =>
        {
            panel.gameObject.SetActive(false);
        });
    }

    /// <summary>
    /// Clears all child card slots from the card parent except the template.
    /// </summary>
    private void ClearCardParent()
    {
        foreach (Transform child in cardParent)
        {
            if (child.GetComponent<CardRevealSlotUI>() != cardRevealSlotTemplate)
            {
                Destroy(child.gameObject);
            }
        }
    }

    /// <summary>
    /// Retrieves a random card from the CardDatabase.
    /// </summary>
    /// <returns>A random CardData object.</returns>
    private CardData GetRandomCardData()
    {
        int randomIndex = UnityEngine.Random.Range(0, cardDatabaseSO.CardDatas.Count);
        return cardDatabaseSO.CardDatas[randomIndex];
    }
}
