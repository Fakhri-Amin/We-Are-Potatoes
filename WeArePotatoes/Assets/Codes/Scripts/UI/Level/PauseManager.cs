using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
    [SerializeField] private PauseUI pauseUI;
    [SerializeField] private Button pauseButton;

    private void Awake()
    {
        pauseButton.onClick.AddListener(PauseGame);
        pauseUI.InitializeButtonFunction(ResumeGame);
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
        pauseUI.Hide();
        Time.timeScale = 1;
    }
}
