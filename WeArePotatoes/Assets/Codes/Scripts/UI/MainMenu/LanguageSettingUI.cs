using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Farou.Utility;
using Sirenix.OdinInspector;
using MoreMountains.Feedbacks;
using MoreMountains.Tools;
using DG.Tweening;

public class LanguageSettingUI : MonoBehaviour
{
    [SerializeField] private CanvasGroup panel;
    [SerializeField] private Button closeButton;
    [SerializeField] private Image englishCheckImage;
    [SerializeField] private Image japaneseCheckImage;
    [SerializeField] private Image koreanCheckImage;
    [SerializeField] private Image bahasaIndonesiaCheckImage;

    private void Awake()
    {
        closeButton.onClick.AddListener(() =>
        {
            Hide();
        });
    }

    private void Start()
    {
        panel.gameObject.SetActive(false);
    }

    /// <summary>
    /// Displays the shop UI by fading in the panel.
    /// </summary>
    public void Show()
    {
        AudioManager.Instance.PlayClickFeedbacks();

        panel.gameObject.SetActive(true);
        panel.alpha = 0;
        panel.DOFade(1, 0.1f); // Fade in effect
    }

    /// <summary>
    /// Hides the shop UI by fading out the panel.
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
}
