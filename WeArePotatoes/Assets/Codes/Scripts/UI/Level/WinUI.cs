using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class WinUI : MonoBehaviour
{
    [SerializeField] private TMP_Text coinCollectedText;
    [SerializeField] private Button continueButton;
    [SerializeField] private CanvasGroup popup;

    public void Show(int coinCollectedAmount, Action onContinueButtonClicked)
    {
        // popup.alpha = 0;
        popup.gameObject.SetActive(true);
        popup.DOFade(1, 0.1f);

        coinCollectedText.text = "+" + coinCollectedAmount;
        continueButton.onClick.RemoveAllListeners();
        continueButton.onClick.AddListener(() => { onContinueButtonClicked?.Invoke(); });
    }

    public void Hide()
    {
        // popup.alpha = 1;
        popup.DOFade(0, 0.1f).OnComplete(() =>
        {
            popup.gameObject.SetActive(false);
        });
    }
}
