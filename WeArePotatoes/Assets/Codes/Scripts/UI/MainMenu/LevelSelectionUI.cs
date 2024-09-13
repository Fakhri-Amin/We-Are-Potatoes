using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using MoreMountains.Feedbacks;

public class LevelSelectionUI : MonoBehaviour
{
    [SerializeField] private Transform panel;
    [SerializeField] private Button[] levelButtons;
    [SerializeField] private MMFeedbacks loadGameSceneFeedbacks;

    private LevelWaveDatabaseSO levelWaveDatabaseSO;

    private void Start()
    {
        levelWaveDatabaseSO = GameDataManager.Instance.LevelWaveDatabaseSO;

        for (int i = 0; i < levelWaveDatabaseSO.LevelWaveSOs.Count; i++)
        {
            int index = i; // Store the current index
            levelButtons[i].onClick.AddListener(() =>
            {
                // Debug.Log(levelWaveDatabaseSO.LevelWaveSOs[index].LevelIndex);
                GameDataManager.Instance.SetSelectedLevel(levelWaveDatabaseSO.LevelWaveSOs[index].LevelIndex);
                loadGameSceneFeedbacks.PlayFeedbacks();
            });
        }

        Hide();
    }

    public void Show()
    {
        panel.gameObject.SetActive(true);
    }

    public void Hide()
    {
        panel.gameObject.SetActive(false);
    }
}
