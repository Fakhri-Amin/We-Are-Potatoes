using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
using System.Linq;

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
    private int rewardIndex = 0;
    private float timePassed;

    public LevelWaveSO CurrentLevelWave => currentLevelWave;
    public BaseBuildingSO BaseBuildingSO => baseBuildingSO;

    private void Awake()
    {
        coinManager = GetComponent<CoinManager>();
        selectedLevelMap = GameDataManager.Instance.SelectedLevelMap;
        currentLevelWave = levelWaveDatabaseSO.MapLevelReferences.Find(i => i.MapType == selectedLevelMap.MapType)
                                                        .Levels[selectedLevelMap.SelectedLevelIndex];
    }

    private void Start()
    {
        InitializeLevelGraphics();
        InitializeBases();
        waveProgressionBar.maxValue = currentLevelWave.DelayBetweenWaves * currentLevelWave.WaveDatas.Count;

        // Hide UI initially
        winUI.Hide();
        loseUI.Hide();
        unitLevelRewardUI.Hide();
    }

    private void InitializeLevelGraphics()
    {
        WorldSpriteReference worldSpriteReference = gameAssetSO.WorldSpriteReferences
            .Find(i => i.MapType == currentLevelWave.MapType);
        worldRenderer.sprite = worldSpriteReference.LevelMapSprites[(int)currentLevelWave.LevelMapType];
        groundRenderer.color = worldSpriteReference.GroundColor;
    }

    private void InitializeBases()
    {
        playerBase.Initialize(baseBuildingSO.BaseHealth + GameDataManager.Instance.GetTotalBaseHealthPercentage());
        enemyBase.Initialize(CurrentLevelWave.BaseHealth);
    }

    private void Update()
    {
        if (waveProgressionBar.value < waveProgressionBar.maxValue)
        {
            timePassed += Time.deltaTime;
            waveProgressionBar.value = timePassed;
        }
    }

    public IEnumerator HandleLevelWin()
    {
        yield return HideInGameHUDAndWait();

        ShowWinUI();

        int highestLevelNumber = GameDataManager.Instance.GetCompletedLevelsForMap(selectedLevelMap.MapType).Max();
        bool hasCompletedAllLevels = highestLevelNumber == levelWaveDatabaseSO.MapLevelReferences
            .Find(i => i.MapType == selectedLevelMap.MapType).Levels.Count - 1;

        GameDataManager.Instance.AddNewCompletedLevel(selectedLevelMap.MapType, selectedLevelMap.SelectedLevelIndex, hasCompletedAllLevels);

        CollectCurrencyRewards();
    }

    public IEnumerator HandleLevelLose()
    {
        yield return HideInGameHUDAndWait();

        loseUI.Show(coinManager.CoinCollected, LoadMainMenu);

        CollectCurrencyRewards();
    }

    private void CollectCurrencyRewards()
    {
        CurrencyType currencyType = currentLevelWave.MapType == MapType.Dungeon ? CurrencyType.AzureCoin : CurrencyType.GoldCoin;
        GameDataManager.Instance.SetCoinCollected(currencyType, coinManager.CoinCollected);
    }

    private void ShowWinUI()
    {
        CurrencyType currencyType = currentLevelWave.MapType == MapType.Dungeon ? CurrencyType.AzureCoin : CurrencyType.GoldCoin;
        winUI.Show(currencyType, coinManager.CoinCollected, OnWinUIContinue);
    }

    private void OnWinUIContinue()
    {
        rewardIndex = 0;
        ShowNextUnitReward();
    }

    private void ShowNextUnitReward()
    {
        if (rewardIndex >= currentLevelWave.UnitRewardList.Count)
        {
            LoadMainMenu();
            return;
        }

        var currentReward = currentLevelWave.UnitRewardList[rewardIndex];

        if (GameDataManager.Instance.IsUnitAlreadyUnlocked(currentReward))
        {
            rewardIndex++;
            ShowNextUnitReward();
        }
        else
        {
            UnlockAndShowReward(currentReward);
        }
    }

    private void UnlockAndShowReward(UnitHero currentReward)
    {
        GameDataManager.Instance.AddUnlockedUnit(currentReward); // Unlock the unit

        fader.DOFade(1, 0.1f).OnComplete(() =>
        {
            winUI.Hide();
            UnitData unitData = unitDataSO.UnitStatDataList.Find(i => i.UnitHero == currentReward);
            unitLevelRewardUI.Show(unitData, () =>
            {
                rewardIndex++;
                ShowNextUnitReward();
            });
            fader.DOFade(0, 0.1f);
        });
    }

    private IEnumerator HideInGameHUDAndWait()
    {
        HideInGameHUD();
        yield return new WaitForSeconds(waitTimeBeforeShowingUI);
    }

    private void HideInGameHUD()
    {
        inGameHUD.blocksRaycasts = false;
        inGameHUD.DOFade(0, 0.1f);
    }

    private void LoadMainMenu()
    {
        loadMainMenuFeedbacks.PlayFeedbacks();
    }
}
