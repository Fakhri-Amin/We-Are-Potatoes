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
    [SerializeField] private Image cardIcon;
    [SerializeField] private TMP_Text cardLevelText;

    public void Initialize(ObtainedCard obtainedCard, Sprite cardSprite)
    {
        cardIcon.sprite = cardSprite;
        cardLevelText.text = "Level " + obtainedCard.Level;
    }
}
