using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private LevelWaveDatabaseSO levelWaveDatabaseSO;
    [SerializeField] private UnitDataSO unitDataSO;
    [SerializeField] private WinUI winUI;
    [SerializeField] private LoseUI loseUI;
    [SerializeField] private UnitLevelRewardUI unitLevelRewardUI;

    private LevelWaveSO currentLevelWave;
    private CoinManager coinManager;

    public LevelWaveSO CurrentLevelWave => currentLevelWave;

    private void Awake()
    {
        // Assuming CoinManager is a component on the same GameObject
        coinManager = GetComponent<CoinManager>();

        int selectedLevel = GameDataManager.Instance.SelectedLevelIndex;
        currentLevelWave = levelWaveDatabaseSO.LevelWaveSOs.Find(l => l.LevelIndex == selectedLevel);
    }

    private void Start()
    {
        HideAllUI();
    }

    private void HideAllUI()
    {
        winUI.Hide();
        loseUI.Hide();
        unitLevelRewardUI.Hide();
    }

    public void HandleLevelWin()
    {
        if (currentLevelWave.UnitReward == UnitHero.None)
        {
            ShowWinUI();
        }
        else
        {
            ShowUnitRewardUI();
        }

        GameDataManager.Instance.ModifyMoney(coinManager.CoinCollected);
    }

    public void HandleLevelLose()
    {
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
            winUI.Hide();
            UnitData unitData = unitDataSO.UnitStatDataList.Find(i => i.UnitHero == currentLevelWave.UnitReward);
            unitLevelRewardUI.Show(unitData, LoadMainMenu);
        });
    }

    private void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}

