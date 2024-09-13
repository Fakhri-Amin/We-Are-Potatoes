using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using MoreMountains.Feedbacks;

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
    [SerializeField] private Color unlockedColor;
    [SerializeField] private Color lockedColor;
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

        // Iterate through the unlocked levels and enable interaction for the unlocked levels
        for (int i = 0; i < Mathf.Min(levelButtonDatas.Count, completedLevelList.Count); i++)
        {
            if (levelButtonDatas[i].LevelIndex == completedLevelList[i])
            {
                levelButtonDatas[i].LevelButton.interactable = true;

                // Enable any additional buttons related to the unlocked level
                foreach (var item in levelButtonDatas[i].UnlockedLevelButtons)
                {
                    item.interactable = true;
                }
            }
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
