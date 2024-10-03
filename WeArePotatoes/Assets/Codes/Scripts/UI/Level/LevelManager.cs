using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
using Microsoft.Unity.VisualStudio.Editor;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private GameAssetSO gameAssetSO;
    [SerializeField] private LevelWaveDatabaseSO levelWaveDatabaseSO;
    [SerializeField] private UnitDataSO unitDataSO;
    [SerializeField] private BaseBuildingSO baseBuildingSO;
    [SerializeField] private WinUI winUI;
    [SerializeField] private LoseUI loseUI;
    [SerializeField] private UnitLevelRewardUI unitLevelRewardUI;
    [SerializeField] private MMFeedbacks loadMainMenuFeedbacks;
    [SerializeField] private SpriteRenderer worldRenderer;
    [SerializeField] private SpriteRenderer groundRenderer;

    [Header("Base Buildings")]
    [SerializeField] private BaseBuilding playerBase;
    [SerializeField] private BaseBuilding enemyBase;
    [SerializeField] private float waitTimeBeforeShowingUI = 3f;

    [Header("Other Reference")]
    [SerializeField] private CanvasGroup inGameHUD;
    [SerializeField] private CanvasGroup fader;
    [SerializeField] private Slider waveProgressionBar;

    private SelectedLevelMap selectedLevelMap;
    private LevelWaveSO currentLevelWave;
    private CoinManager coinManager;
    private int rewardIndex = 0; // Track the current reward index
    private float timePassed;

    public LevelWaveSO CurrentLevelWave => currentLevelWave;
    public BaseBuildingSO BaseBuildingSO => baseBuildingSO;

    private void Awake()
    {
        coinManager = GetComponent<CoinManager>();
        selectedLevelMap = GameDataManager.Instance.SelectedLevelMap;
        currentLevelWave = levelWaveDatabaseSO.MapLevelReferences.Find(i => i.MapType == selectedLevelMap.MapType).Levels[selectedLevelMap.SelectedLevelIndex];
    }

    private void Start()
    {
        WorldSpriteReference worldSpriteReference = gameAssetSO.WorldSpriteReferences.Find(i => i.MapType == currentLevelWave.MapType);
        worldRenderer.sprite = worldSpriteReference.LevelMapSprites[(int)currentLevelWave.LevelMapType];
        groundRenderer.color = worldSpriteReference.GroundColor;

        playerBase.Initialize(baseBuildingSO.BaseHealth);
        enemyBase.Initialize(CurrentLevelWave.BaseHealth);

        waveProgressionBar.maxValue = currentLevelWave.DelayBetweenWaves * currentLevelWave.WaveDatas.Count;

        HideAllUI();
    }

    private void Update()
    {
        if (waveProgressionBar.value >= waveProgressionBar.maxValue) return;
        timePassed += Time.deltaTime;
        waveProgressionBar.value = timePassed;
    }

    private void HideAllUI()
    {
        winUI.Hide();
        loseUI.Hide();
        unitLevelRewardUI.Hide();
    }

    public IEnumerator HandleLevelWin()
    {
        HideInGameHUD();

        yield return new WaitForSeconds(waitTimeBeforeShowingUI);

        // Step 1: Show Win UI first, no matter what
        ShowWinUI();

        bool hasCompletedAllLevels = selectedLevelMap.SelectedLevelIndex == levelWaveDatabaseSO.MapLevelReferences.Find(i => i.MapType == selectedLevelMap.MapType).Levels.Count - 1;
        GameDataManager.Instance.AddNewCompletedLevel(selectedLevelMap.MapType, selectedLevelMap.SelectedLevelIndex, hasCompletedAllLevels);
        GameDataManager.Instance.ModifyMoney(coinManager.CoinCollected);
    }

    public IEnumerator HandleLevelLose()
    {
        HideInGameHUD();

        yield return new WaitForSeconds(waitTimeBeforeShowingUI);

        loseUI.Show(coinManager.CoinCollected, LoadMainMenu);
        GameDataManager.Instance.ModifyMoney(coinManager.CoinCollected);
    }

    private void ShowWinUI()
    {
        winUI.Show(coinManager.CoinCollected, OnWinUIContinue);
    }

    private void OnWinUIContinue()
    {
        // After clicking continue in the Win UI, start processing rewards
        rewardIndex = 0;
        ShowNextUnitReward();
    }

    private void ShowNextUnitReward()
    {
        if (rewardIndex >= currentLevelWave.UnitRewardList.Count)
        {
            // If we've shown all rewards, proceed to the Main Menu
            LoadMainMenu();
            return;
        }

        var currentReward = currentLevelWave.UnitRewardList[rewardIndex];

        if (GameDataManager.Instance.IsUnitAlreadyUnlocked(currentReward))
        {
            // Skip if already unlocked and check the next reward
            rewardIndex++;
            ShowNextUnitReward();
        }
        else
        {
            // If the reward is not unlocked, show the reward UI
            GameDataManager.Instance.AddUnlockedUnit(currentReward);  // Unlock it

            fader.DOFade(1, 0.1f).OnComplete(() =>
            {
                winUI.Hide();
                UnitData unitData = unitDataSO.UnitStatDataList.Find(i => i.UnitHero == currentReward);
                unitLevelRewardUI.Show(unitData, () =>
                {
                    // After showing the reward, move to the next
                    rewardIndex++;
                    ShowNextUnitReward();
                });
                fader.DOFade(0, 0.1f);
            });
        }
    }

    private void LoadMainMenu()
    {
        // SceneManager.LoadScene("MainMenu");
        loadMainMenuFeedbacks.PlayFeedbacks();
    }

    private void HideInGameHUD()
    {
        inGameHUD.blocksRaycasts = false;
        inGameHUD.DOFade(0, 0.1f);
    }
}
