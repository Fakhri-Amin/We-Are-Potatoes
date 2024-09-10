using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class LoseUI : MonoBehaviour
{
    [SerializeField] private TMP_Text coinCollectedText;
    [SerializeField] private Button continueButton;
    [SerializeField] private Transform popup;

    public void Show(int coinCollectedAmount, Action onContinueButtonClicked)
    {
        popup.gameObject.SetActive(true);

        coinCollectedText.text = "+" + coinCollectedAmount;
        continueButton.onClick.AddListener(() => { onContinueButtonClicked?.Invoke(); });
    }

    public void Hide()
    {
        popup.gameObject.SetActive(false);
    }
}
