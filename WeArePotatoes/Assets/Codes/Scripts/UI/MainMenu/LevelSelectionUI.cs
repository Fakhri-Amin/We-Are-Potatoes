using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MoreMountains.Feedbacks;
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

    [SerializeField] private Transform[] mapTransforms;
    [SerializeField] private Transform mapButtonTransform;
    [SerializeField] private Color completedColor;
    [SerializeField] private Color unlockedColor;
    [SerializeField] private Button previousMapButton;
    [SerializeField] private Button nextMapButton;
    [SerializeField] private MMFeedbacks loadGameSceneFeedbacks;
    [SerializeField] private CanvasGroup fader;

    [Header("Map Level Button Config")]
    // List of maps and their associated button data
    [SerializeField] private List<MapLevelButtonConfig> mapLevelButtonConfigs = new List<MapLevelButtonConfig>();

    private LevelWaveDatabaseSO levelWaveDatabaseSO;
    private int currentMapIndex;

    [System.Serializable]
    public class MapLevelButtonConfig
    {
        public MapType MapType;
        public List<LevelButtonData> LevelButtonDatas = new List<LevelButtonData>();
    }

    private void Awake()
    {
        previousMapButton.onClick.AddListener(OpenPreviousMap);
        nextMapButton.onClick.AddListener(OpenNextMap);
    }

    private void Start()
    {
        levelWaveDatabaseSO = GameDataManager.Instance?.LevelWaveDatabaseSO;

        foreach (var item in GameDataManager.Instance.CompletedLevelMapList)
        {
            if (!item.HasCompletedAllLevels)
            {
                currentMapIndex = (int)item.MapType;
                break;
            }
        }

        if (levelWaveDatabaseSO == null)
        {
            Debug.LogError("LevelWaveDatabaseSO is not set!");
            return;
        }

        // Loop through each map configuration
        foreach (var mapConfig in mapLevelButtonConfigs)
        {
            MapType currentMap = mapConfig.MapType;

            foreach (var levelButtonData in mapConfig.LevelButtonDatas)
            {
                // Disable all buttons by default
                levelButtonData.LevelButton.interactable = false;
            }

            List<int> completedLevelList = GameDataManager.Instance?.CompletedLevelMapList
                .Find(i => i.MapType == currentMap)?.CompletedLevelList;

            if (completedLevelList == null)
            {
                Debug.LogWarning($"CompletedLevelList is null for map type: {currentMap}");

                if (currentMap != MapType.Beach) continue;

                // Enable the first button if no levels are completed
                mapLevelButtonConfigs[0].LevelButtonDatas[0].LevelButton.interactable = true;
                mapLevelButtonConfigs[0].LevelButtonDatas[0].LevelButton.GetComponent<Image>().color = unlockedColor;
                mapLevelButtonConfigs[0].LevelButtonDatas[0].LevelButton.onClick.AddListener(() =>
                {
                    AudioManager.Instance.PlayClickFeedbacks();

                    // Set the selected level for this map
                    GameDataManager.Instance.SetSelectedLevel(MapType.Beach, 0);
                    loadGameSceneFeedbacks?.PlayFeedbacks(); // Null check before playing feedback
                });

                continue;
            }

            var levelReference = levelWaveDatabaseSO.MapLevelReferences.Find(i => i.MapType == currentMap);
            if (levelReference == null)
            {
                Debug.LogWarning($"No levels found for map type: {currentMap}");
                continue;
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

        }

        Hide(); // Initially hide the level selection panel
    }

    public void Show()
    {
        mapButtonTransform.gameObject.SetActive(true);
        mapTransforms[currentMapIndex].gameObject.SetActive(true);
    }

    public void Hide()
    {
        mapButtonTransform.gameObject.SetActive(false);
        foreach (var item in mapTransforms)
        {
            item.gameObject.SetActive(false);
        }
    }

    public void OpenNextMap()
    {
        if (currentMapIndex + 1 >= mapTransforms.Length) return;

        Hide();
        currentMapIndex++;
        fader.DOFade(1, 0.1f).OnComplete(() =>
        {
            Show();

            fader.DOFade(0, 0.1f);
        });

    }

    public void OpenPreviousMap()
    {
        if (currentMapIndex - 1 < 0) return;

        Hide();
        currentMapIndex--;
        fader.DOFade(1, 0.1f).OnComplete(() =>
        {
            Show();

            fader.DOFade(0, 0.1f);
        });
    }
}
