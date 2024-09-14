using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WinUI : MonoBehaviour
{
    [SerializeField] private TMP_Text coinCollectedText;
    [SerializeField] private Button continueButton;
    [SerializeField] private Transform popup;

    public void Show(int coinCollectedAmount, Action onContinueButtonClicked)
    {
        popup.gameObject.SetActive(true);

        coinCollectedText.text = "+" + coinCollectedAmount;
        continueButton.onClick.RemoveAllListeners();
        continueButton.onClick.AddListener(() => { onContinueButtonClicked?.Invoke(); });
    }

    public void Hide()
    {
        popup.gameObject.SetActive(false);
    }
}
