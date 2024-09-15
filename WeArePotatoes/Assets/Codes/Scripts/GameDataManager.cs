using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Farou.Utility;

[DefaultExecutionOrder(-99999999)]
public class GameDataManager : MonoBehaviour
{
    public static GameDataManager Instance { get; private set; }
    public event Action<int> OnCoinUpdated;
    public event Action<List<UnitHero>> OnSelectedUnitListChanged;
    public event Action<float, float> OnSeedProductionRateChanged;
    public event Action<float, float> OnBaseHealthChanged;

    public UnitDataSO UnitDataSO;
    public LevelWaveDatabaseSO LevelWaveDatabaseSO;
    public BaseBuildingSO BaseBuildingSO;
    public List<UnitHero> SelectedUnitList = new List<UnitHero>(3);
    public List<UnitHero> UnlockedUnitList = new List<UnitHero>();
    public List<int> CompletedLevelList = new List<int>();
    public int Coin;
    public int SelectedLevelIndex = 0;
    public float SeedProductionRate;
    public float BaseHealth;
    public float UpgradeSeedProductionRatePrice;
    public float UpgradeBaseHealthPrice;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(this);

        var gameData = Data.Get<GameData>();

        Coin = gameData.Coin;
        OnCoinUpdated?.Invoke(Coin);

        SelectedUnitList = gameData.SelectedUnitList;
        OnSelectedUnitListChanged?.Invoke(SelectedUnitList);

        UnlockedUnitList = gameData.UnlockedUnitList;
        CompletedLevelList = gameData.CompletedLevelList;

        UpdateSeedProductionRate();
        UpdateBaseHealth();
    }

    private void Start()
    {
        // Set default data
        AddUnlockedUnit(UnitHero.Sword);

        if (SelectedUnitList.Count <= 1)
        {
            List<UnitHero> unitHeroes = new List<UnitHero>
            {
                UnitHero.Sword,
                UnitHero.None,
                UnitHero.None
            };
            SetSelectedUnit(unitHeroes);
        }
    }

    public void Save()
    {
        Data.Save();
    }

    public void SelectLevel(int levelNumber)
    {
        Data.Get<GameData>().SelectedLevel = levelNumber;
        Save();
    }

    public void ModifyMoney(float amount)
    {
        Data.Get<GameData>().Coin += (int)amount;
        Coin = Data.Get<GameData>().Coin;
        OnCoinUpdated?.Invoke(Coin);
        Save();
    }

    public void AddUnlockedUnit(UnitHero unitHero)
    {
        if (UnlockedUnitList.Contains(unitHero)) return;

        Data.Get<GameData>().UnlockedUnitList.Add(unitHero);
        Save();
    }

    public bool IsUnitAlreadyUnlocked(UnitHero unitHero)
    {
        if (UnlockedUnitList.Contains(unitHero))
        {
            return true;
        }
        return false;
    }

    public void SetSelectedUnit(List<UnitHero> selectedUnitList)
    {
        SelectedUnitList = selectedUnitList;
        Data.Get<GameData>().SelectedUnitList = selectedUnitList;
        OnSelectedUnitListChanged?.Invoke(SelectedUnitList);
        Save();
    }

    public void SetSelectedLevel(int levelIndex)
    {
        SelectedLevelIndex = levelIndex;
    }

    public void AddNewCompletedLevel(int levelIndex)
    {
        if (CompletedLevelList.Contains(levelIndex)) return;

        CompletedLevelList.Add(levelIndex);
        Data.Get<GameData>().CompletedLevelList = CompletedLevelList;
        Save();
    }

    public void UpdateSeedProductionRate()
    {
        var gameData = Data.Get<GameData>();
        SeedProductionRate = BaseBuildingSO.SeedProductionRate + (gameData.SeedProductionLevel - 1) * BaseBuildingSO.SeedProductionRateUpgradeAmount;
        UpgradeSeedProductionRatePrice = BaseBuildingSO.UpgradeSeedProductionRatePriceList[gameData.SeedProductionLevel - 1];
        OnSeedProductionRateChanged?.Invoke(SeedProductionRate, UpgradeSeedProductionRatePrice);

        Save();
    }

    public void UpgradeSeedProductionRate()
    {
        var gameData = Data.Get<GameData>();
        gameData.SeedProductionLevel++;

        UpdateSeedProductionRate();
    }

    public void UpdateBaseHealth()
    {
        var gameData = Data.Get<GameData>();
        BaseHealth = BaseBuildingSO.BaseHealth + (gameData.BaseHealthLevel - 1) * BaseBuildingSO.BaseHealthUpgradeAmount;
        UpgradeBaseHealthPrice = BaseBuildingSO.UpgradeBaseHealthPriceList[gameData.BaseHealthLevel - 1];
        OnBaseHealthChanged?.Invoke(BaseHealth, UpgradeBaseHealthPrice);

        Save();
    }

    public void UpgradeBaseHealth()
    {
        var gameData = Data.Get<GameData>();
        gameData.BaseHealthLevel++;

        UpdateBaseHealth();
    }
}
