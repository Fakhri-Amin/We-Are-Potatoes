using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private LevelWaveDatabaseSO levelWaveDatabaseSO;
    [SerializeField] private UnitDataSO unitDataSO;
    [SerializeField] private BaseBuildingSO baseBuildingSO;
    [SerializeField] private WinUI winUI;
    [SerializeField] private LoseUI loseUI;
    [SerializeField] private UnitLevelRewardUI unitLevelRewardUI;
    [SerializeField] private MMFeedbacks loadMainMenuFeedbacks;

    [Header("Base Buildings")]
    [SerializeField] private BaseBuilding playerBase;
    [SerializeField] private BaseBuilding enemyBase;
    [SerializeField] private float waitTimeBeforeShowingUI = 3f;

    [Header("Other Reference")]
    [SerializeField] private CanvasGroup fader;

    private int currentLevelIndex;
    private LevelWaveSO currentLevelWave;
    private CoinManager coinManager;

    public LevelWaveSO CurrentLevelWave => currentLevelWave;

    private void Awake()
    {
        // Assuming CoinManager is a component on the same GameObject
        coinManager = GetComponent<CoinManager>();

        currentLevelIndex = GameDataManager.Instance.SelectedLevelIndex;
        currentLevelWave = levelWaveDatabaseSO.LevelWaveSOs.Find(l => l.LevelIndex == currentLevelIndex);
    }

    private void Start()
    {
        playerBase.Initialize(baseBuildingSO.BaseHealth);
        enemyBase.Initialize(CurrentLevelWave.BaseHealth);

        HideAllUI();
    }

    private void HideAllUI()
    {
        winUI.Hide();
        loseUI.Hide();
        unitLevelRewardUI.Hide();
    }

    public IEnumerator HandleLevelWin()
    {
        yield return new WaitForSeconds(waitTimeBeforeShowingUI);

        if (currentLevelWave.UnitReward == UnitHero.None || GameDataManager.Instance.IsUnitAlreadyUnlocked(currentLevelWave.UnitReward))
        {
            ShowWinUI();
        }
        else
        {
            ShowUnitRewardUI();
            GameDataManager.Instance.AddUnlockedUnit(currentLevelWave.UnitReward);
        }

        GameDataManager.Instance.ModifyMoney(coinManager.CoinCollected);
        GameDataManager.Instance.AddNewCompletedLevel(currentLevelIndex);
    }

    public IEnumerator HandleLevelLose()
    {
        yield return new WaitForSeconds(waitTimeBeforeShowingUI);

        loseUI.Show(coinManager.CoinCollected, LoadMainMenu);
        GameDataManager.Instance.ModifyMoney(coinManager.CoinCollected);
    }

    private void ShowWinUI()
    {
        winUI.Show(coinManager.CoinCollected, LoadMainMenu);
    }

    private void ShowUnitRewardUI()
    {
        winUI.Show(coinManager.CoinCollected, () =>
        {
            fader.DOFade(1, 0.1f).OnComplete(() =>
            {
                winUI.Hide();
                UnitData unitData = unitDataSO.UnitStatDataList.Find(i => i.UnitHero == currentLevelWave.UnitReward);
                unitLevelRewardUI.Show(unitData, LoadMainMenu);

                fader.DOFade(0, 0.1f);
            });

        });
    }

    private void LoadMainMenu()
    {
        // SceneManager.LoadScene("MainMenu");
        loadMainMenuFeedbacks.PlayFeedbacks();
    }
}

