using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using MoreMountains.Feedbacks;
using Sirenix.Utilities;
using MoreMountains.Tools;
using DG.Tweening;

public class LevelSelectionUI : MonoBehaviour
{
    [System.Serializable]
    public class LevelButtonData
    {
        public int LevelIndex;
        public Button LevelButton;
        public Button[] UnlockedLevelButtons;
    }

    [SerializeField] private Transform panel;
    [SerializeField] private Button[] levelButtons;
    [SerializeField] private MMFeedbacks loadGameSceneFeedbacks;

    [Header("Level Button Config")]
    [SerializeField] private List<LevelButtonData> levelButtonDatas = new List<LevelButtonData>();

    private LevelWaveDatabaseSO levelWaveDatabaseSO;

    private void Start()
    {
        levelWaveDatabaseSO = GameDataManager.Instance?.LevelWaveDatabaseSO;
        List<int> completedLevelList = GameDataManager.Instance?.CompletedLevelList;

        if (levelWaveDatabaseSO == null || completedLevelList == null)
        {
            Debug.LogError("LevelWaveDatabaseSO or UnlockedLevelList is not set!");
            return;
        }

        // Limit the iteration to the minimum size of levelButtons and LevelWaveSOs to avoid index out of bounds
        int buttonCount = Mathf.Min(levelButtons.Length, levelWaveDatabaseSO.LevelWaveSOs.Count);

        for (int i = 0; i < buttonCount; i++)
        {
            int index = i; // Capture current index for the lambda

            // Assign button click listeners for all levels
            levelButtons[i].onClick.AddListener(() =>
            {
                GameDataManager.Instance.SetSelectedLevel(levelWaveDatabaseSO.LevelWaveSOs[index].LevelIndex);
                loadGameSceneFeedbacks?.PlayFeedbacks(); // Null check before playing feedback
            });

            levelButtons[i].interactable = false; // Initially set all buttons to non-interactable
        }

        // Iterate through all levelButtonDatas and check if their LevelIndex is in the completedLevelList
        foreach (var levelButtonData in levelButtonDatas)
        {
            if (completedLevelList.Contains(levelButtonData.LevelIndex))
            {
                // Set the level button to be interactable
                levelButtonData.LevelButton.interactable = true;

                // Enable any additional buttons related to the unlocked level
                foreach (var unlockedButton in levelButtonData.UnlockedLevelButtons)
                {
                    unlockedButton.interactable = true;
                }
            }
        }

        if (completedLevelList.Count <= 0 || completedLevelList.IsNullOrEmpty())
        {
            levelButtonDatas[0].LevelButton.interactable = true;
        }

        Hide(); // Initially hide the level selection panel
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
