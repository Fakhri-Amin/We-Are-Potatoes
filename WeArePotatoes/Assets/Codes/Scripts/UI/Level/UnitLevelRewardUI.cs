using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnitLevelRewardUI : MonoBehaviour
{
    [SerializeField] private Transform popup;
    [SerializeField] private TMP_Text unitNameText;
    [SerializeField] private Button continueButton;
    [SerializeField] private Image unitImage;

    public void Show(UnitData unitData, Action onContinueButtonClicked)
    {
        popup.gameObject.SetActive(true);

        unitNameText.text = unitData.Name;
        unitImage.sprite = unitData.Sprite;
        continueButton.onClick.AddListener(() => { onContinueButtonClicked?.Invoke(); });
    }

    public void Hide()
    {
        popup.gameObject.SetActive(false);
    }
}
