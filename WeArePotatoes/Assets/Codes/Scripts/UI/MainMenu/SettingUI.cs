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

public class SettingUI : MonoBehaviour
{
    [SerializeField] private CanvasGroup panel;
    [SerializeField] private Button closeButton;

    [Header("SFX")]
    [SerializeField] private Button sfxButton;
    [SerializeField] private Image sfxHandleImage;
    [SerializeField] private Image sfxBackgroundImage;
    [SerializeField] private Color sfxBackgroundActiveColor;
    [SerializeField] private Color sfxBackgroundInActiveColor;
    [SerializeField] private MMFeedbacks muteFeedbacks;
    [SerializeField] private MMFeedbacks unmuteFeedbacks;
    private bool isSFXMute;

    [Header("Language")]
    [SerializeField] private LanguageSettingUI languageSettingUI;
    [SerializeField] private Button languageButton;

    private void Awake()
    {
        sfxButton.onClick.AddListener(OnToggleSFXButton);

        languageButton.onClick.AddListener(OpenLanguageMenu);

        closeButton.onClick.AddListener(() =>
        {
            Hide();
        });
    }

    private void Start()
    {
        bool savedSFXSettingState = GameDataManager.Instance.GetSFXSettingState();
        if (savedSFXSettingState)
        {
            MuteSFX();
        }
        else
        {
            UnMuteSFX();
        }

        panel.transform.gameObject.SetActive(false);
    }

    private void OnToggleSFXButton()
    {
        AudioManager.Instance.PlayClickFeedbacks();

        isSFXMute = !isSFXMute;

        if (isSFXMute)
        {
            MuteSFX();
        }
        else
        {
            UnMuteSFX();
        }
    }

    private void MuteSFX()
    {
        sfxHandleImage.transform.DOLocalMoveX(-35, 0.1f);
        sfxBackgroundImage.color = sfxBackgroundInActiveColor;
        muteFeedbacks.PlayFeedbacks();

        GameDataManager.Instance.MuteSFX();
    }

    private void UnMuteSFX()
    {
        sfxHandleImage.transform.DOLocalMoveX(35, 0.1f);
        sfxBackgroundImage.color = sfxBackgroundActiveColor;
        unmuteFeedbacks.PlayFeedbacks();

        GameDataManager.Instance.UnMuteSFX();
    }

    private void OpenLanguageMenu()
    {
        languageSettingUI.Show();
    }

    /// <summary>
    /// Displays the shop UI by fading in the panel.
    /// </summary>
    public void Show()
    {
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
