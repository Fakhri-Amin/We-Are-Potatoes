using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MoreMountains.Feedbacks;
using MoreMountains.Tools;

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
    [SerializeField] private Color completedColor;
    [SerializeField] private Color unlockedColor;
    [SerializeField] private MMFeedbacks loadGameSceneFeedbacks;

    [Header("Map Level Button Config")]
    // List of maps and their associated button data
    [SerializeField] private List<MapLevelButtonConfig> mapLevelButtonConfigs = new List<MapLevelButtonConfig>();

    private LevelWaveDatabaseSO levelWaveDatabaseSO;

    [System.Serializable]
    public class MapLevelButtonConfig
    {
        public MapType MapType;
        public List<LevelButtonData> LevelButtonDatas = new List<LevelButtonData>();
    }

    private void Start()
    {
        levelWaveDatabaseSO = GameDataManager.Instance?.LevelWaveDatabaseSO;

        if (levelWaveDatabaseSO == null)
        {
            Debug.LogError("LevelWaveDatabaseSO is not set!");
            return;
        }

        // Loop through each map configuration
        foreach (var mapConfig in mapLevelButtonConfigs)
        {
            MapType currentMap = mapConfig.MapType;

            List<int> completedLevelList = GameDataManager.Instance?.CompletedLevelMapList
                .Find(i => i.MapType == currentMap)?.CompletedLevelList;

            if (completedLevelList == null)
            {
                Debug.LogWarning($"CompletedLevelList is null for map type: {currentMap}");
                continue;
            }

            var levelReference = levelWaveDatabaseSO.MapLevelReferences.Find(i => i.MapType == currentMap);
            if (levelReference == null)
            {
                Debug.LogWarning($"No levels found for map type: {currentMap}");
                continue;
            }

            foreach (var levelButtonData in mapConfig.LevelButtonDatas)
            {
                // Disable all buttons by default
                levelButtonData.LevelButton.interactable = false;
            }

            // Iterate through each level button data for the current map
            foreach (var levelButtonData in mapConfig.LevelButtonDatas)
            {
                // Assign button click listeners for the levels
                levelButtonData.LevelButton.onClick.AddListener(() =>
                {
                    AudioManager.Instance.PlayClickFeedbacks();

                    // Set the selected level for this map
                    GameDataManager.Instance.SetSelectedLevel(currentMap, levelButtonData.LevelIndex);
                    loadGameSceneFeedbacks?.PlayFeedbacks(); // Null check before playing feedback
                });

                // Enable completed levels and unlock their additional buttons
                if (completedLevelList.Contains(levelButtonData.LevelIndex))
                {
                    levelButtonData.LevelButton.interactable = true;
                    levelButtonData.LevelButton.GetComponent<Image>().color = completedColor;

                    foreach (var unlockedButton in levelButtonData.UnlockedLevelButtons)
                    {
                        unlockedButton.interactable = true;
                        unlockedButton.GetComponent<Image>().color = unlockedColor;
                    }
                }
            }

            // Enable the first button if no levels are completed
            if (completedLevelList.Count == 0)
            {
                mapConfig.LevelButtonDatas[0].LevelButton.interactable = true;
                mapConfig.LevelButtonDatas[0].LevelButton.GetComponent<Image>().color = unlockedColor;
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
