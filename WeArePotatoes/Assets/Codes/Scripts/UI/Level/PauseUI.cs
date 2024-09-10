using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseUI : MonoBehaviour
{
    [SerializeField] private Transform popup;
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button settingButton;
    [SerializeField] private Button giveUpButton;

    public void InitializeButtonFunction(Action onResumeButtonClicked)
    {
        resumeButton.onClick.AddListener(() => onResumeButtonClicked());
    }

    public void Show()
    {
        popup.gameObject.SetActive(true);
    }

    public void Hide()
    {
        popup.gameObject.SetActive(false);
    }
}
