using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
    [SerializeField] private PauseUI pauseUI;
    [SerializeField] private Button pauseButton;
    [SerializeField] private MMFeedbacks loadMainMenuSceneFeedbacks;

    private void Awake()
    {
        pauseButton.onClick.AddListener(PauseGame);
        pauseUI.InitializeButtonFunction(ResumeGame, GiveUpGame);
    }

    private void Start()
    {
        pauseUI.Hide();
    }

    private void PauseGame()
    {
        pauseUI.Show();
        Time.timeScale = 0;
    }

    private void ResumeGame()
    {
        Time.timeScale = 1;
        pauseUI.Hide();
    }

    private void GiveUpGame()
    {
        Time.timeScale = 1;
        loadMainMenuSceneFeedbacks.PlayFeedbacks();
    }
}

