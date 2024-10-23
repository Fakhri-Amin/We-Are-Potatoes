using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MoreMountains.Feedbacks;
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

    [System.Serializable]
    public class MapLevelButtonConfig
    {
        public MapType MapType;
        public List<LevelButtonData> LevelButtonDatas = new List<LevelButtonData>();
    }

    [Header("Map Configuration")]
    [SerializeField] private Transform[] mapTransforms;
    [SerializeField] private Transform mapButtonTransform;
    [SerializeField] private Color completedColor;
    [SerializeField] private Color unlockedColor;
    [SerializeField] private Button previousMapButton;
    [SerializeField] private Image previousMapButtonIcon;
    [SerializeField] private Button nextMapButton;
    [SerializeField] private Image nextMapButtonIcon;
    [SerializeField] private MMFeedbacks loadGameSceneFeedbacks;
    [SerializeField] private CanvasGroup fader;
    [SerializeField] private Transform lockedMapTransform;

    [Header("Map Level Button Config")]
    [SerializeField] private List<MapLevelButtonConfig> mapLevelButtonConfigs = new List<MapLevelButtonConfig>();

    private LevelWaveDatabaseSO levelWaveDatabaseSO;
    private int currentMapIndex;
    private int uncompletedMapIndex;

    private const float fadeDuration = 0.1f;

    private void Awake()
    {
        previousMapButton.onClick.AddListener(OpenPreviousMap);
        nextMapButton.onClick.AddListener(OpenNextMap);
    }

    private void Start()
    {
        Hide();

        levelWaveDatabaseSO = GameDataManager.Instance?.LevelWaveDatabaseSO;

        if (levelWaveDatabaseSO == null)
        {
            Debug.LogError("LevelWaveDatabaseSO is not set!");
            return;
        }

        InitializeCurrentMapIndex();
        InitializeMapButtons();

        HandleButtonActiveStatus();
    }

    private void InitializeCurrentMapIndex()
    {
        foreach (var item in GameDataManager.Instance.CompletedLevelMapList)
        {
            if (!item.HasCompletedAllLevels)
            {
                currentMapIndex = (int)item.MapType;
                uncompletedMapIndex = (int)item.MapType;

                Debug.Log(currentMapIndex);
                Debug.Log(uncompletedMapIndex);
                break;
            }
        }
    }

    private void InitializeMapButtons()
    {
        foreach (var mapConfig in mapLevelButtonConfigs)
        {
            var currentMap = mapConfig.MapType;
            DisableAllLevelButtons(mapConfig);

            List<int> completedLevelList = GetCompletedLevelsForMap(currentMap);

            if (completedLevelList == null || completedLevelList.Count == 0)
            {
                HandleNoCompletedLevels(mapConfig, currentMap);
                continue;
            }

            SetupCompletedLevels(mapConfig, completedLevelList, currentMap);
        }
    }

    private void DisableAllLevelButtons(MapLevelButtonConfig mapConfig)
    {
        foreach (var levelButtonData in mapConfig.LevelButtonDatas)
        {
            levelButtonData.LevelButton.interactable = false;
        }
    }

    private List<int> GetCompletedLevelsForMap(MapType mapType)
    {
        return GameDataManager.Instance?.GetCompletedLevelsForMap(mapType);
    }

    private void HandleNoCompletedLevels(MapLevelButtonConfig mapConfig, MapType currentMap)
    {
        if (currentMap == MapType.Beach)
        {
            EnableLevelButton(mapConfig.LevelButtonDatas[0], 0, MapType.Beach);
        }
        else if (currentMap == MapType.Forest && IsPreviousMapCompleted(MapType.Beach))
        {
            EnableLevelButton(mapConfig.LevelButtonDatas[0], 0, MapType.Forest);
        }
    }

    private bool IsPreviousMapCompleted(MapType previousMap)
    {
        return GameDataManager.Instance.IsPreviousMapCompleted(previousMap);
    }

    private void EnableLevelButton(LevelButtonData levelButtonData, int levelIndex, MapType mapType)
    {
        levelButtonData.LevelButton.interactable = true;
        levelButtonData.LevelButton.GetComponent<Image>().color = unlockedColor;
        levelButtonData.LevelButton.onClick.AddListener(() =>
        {
            AudioManager.Instance.PlayClickFeedbacks();
            GameDataManager.Instance.SetSelectedLevel(mapType, levelIndex);
            loadGameSceneFeedbacks?.PlayFeedbacks();
        });
    }

    private void SetupCompletedLevels(MapLevelButtonConfig mapConfig, List<int> completedLevelList, MapType currentMap)
    {
        foreach (var levelButtonData in mapConfig.LevelButtonDatas)
        {
            levelButtonData.LevelButton.onClick.AddListener(() =>
            {
                AudioManager.Instance.PlayClickFeedbacks();
                GameDataManager.Instance.SetSelectedLevel(currentMap, levelButtonData.LevelIndex);
                loadGameSceneFeedbacks?.PlayFeedbacks();
            });

            if (completedLevelList.Contains(levelButtonData.LevelIndex))
            {
                EnableCompletedLevel(levelButtonData);
            }
        }
    }

    private void EnableCompletedLevel(LevelButtonData levelButtonData)
    {
        levelButtonData.LevelButton.interactable = true;
        levelButtonData.LevelButton.GetComponent<Image>().color = completedColor;

        foreach (var unlockedButton in levelButtonData.UnlockedLevelButtons)
        {
            unlockedButton.interactable = true;
            unlockedButton.GetComponent<Image>().color = unlockedColor;
        }
    }

    public void Show()
    {
        mapButtonTransform.gameObject.SetActive(true);
        mapTransforms[currentMapIndex].gameObject.SetActive(true);
        HandleLockedMapStatus();
    }

    public void Hide()
    {
        mapButtonTransform.gameObject.SetActive(false);
        lockedMapTransform.gameObject.SetActive(false);
        foreach (var item in mapTransforms)
        {
            item.gameObject.SetActive(false);
        }
    }

    public void OpenNextMap()
    {
        if (currentMapIndex + 1 >= mapTransforms.Length) return;

        ChangeMap(currentMapIndex + 1);
    }

    public void OpenPreviousMap()
    {
        if (currentMapIndex - 1 < 0) return;

        ChangeMap(currentMapIndex - 1);
    }

    private void ChangeMap(int newMapIndex)
    {
        AudioManager.Instance.PlayClickFeedbacks();
        Hide();
        currentMapIndex = newMapIndex;
        HandleButtonActiveStatus();
        HandleLockedMapStatus();

        fader.DOFade(1, fadeDuration).OnComplete(() =>
        {
            Show();
            fader.DOFade(0, fadeDuration);
        });
    }

    private void HandleButtonActiveStatus()
    {
        nextMapButton.interactable = currentMapIndex + 1 < mapTransforms.Length;
        nextMapButtonIcon.color = nextMapButton.interactable ? Color.white : new Color(1, 1, 1, 0.1f);

        previousMapButton.interactable = currentMapIndex > 0;
        previousMapButtonIcon.color = previousMapButton.interactable ? Color.white : new Color(1, 1, 1, 0.1f);
    }

    private void HandleLockedMapStatus()
    {
        if (currentMapIndex <= 0)
        {
            lockedMapTransform.gameObject.SetActive(false);
        }
        else
        {
            bool isMapUnlocked = GameDataManager.Instance.IsPreviousMapCompleted(mapLevelButtonConfigs[currentMapIndex - 1].MapType);
            lockedMapTransform.gameObject.SetActive(!isMapUnlocked);
        }
    }
}
