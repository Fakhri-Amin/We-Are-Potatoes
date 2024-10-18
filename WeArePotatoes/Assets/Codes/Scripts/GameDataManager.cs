using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Farou.Utility;
using System.Linq;
using UnityEngine.Assertions.Must;
using Unity.VisualScripting;

// [DefaultExecutionOrder(-99999999)]
public class GameDataManager : PersistentSingleton<GameDataManager>
{
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
    public List<CompletedLevelMap> CompletedLevelMapList = new List<CompletedLevelMap>();
    public SelectedLevelMap SelectedLevelMap = new SelectedLevelMap();
    public int CoinCollected;
    public int Coin;
    public float SeedProductionRate;
    public float BaseHealth;
    public float UpgradeSeedProductionRatePrice;
    public float UpgradeBaseHealthPrice;
    public bool IsThereNewPotato;

    public new void Awake()
    {
        base.Awake();

        var gameData = Data.Get<GameData>();

        SetInitialDefaultData();

        Coin = gameData.Coin;
        OnCoinUpdated?.Invoke(Coin);

        SelectedUnitList = gameData.SelectedUnitList;
        OnSelectedUnitListChanged?.Invoke(SelectedUnitList);

        UnlockedUnitList = gameData.UnlockedUnitList;
        CompletedLevelMapList = gameData.CompletedLevelMapList;

        UpdateSeedProductionRate();
        UpdateBaseHealth();
    }

    private void SetInitialDefaultData()
    {
        if (SelectedUnitList.Count <= 1)
        {
            // Set default data
            AddDefaultUnlockedUnit(UnitHero.Sword);

            List<UnitHero> unitHeroes = new List<UnitHero>
            {
                UnitHero.Sword,
                UnitHero.None,
                UnitHero.None
            };
            SetSelectedUnit(unitHeroes);
            AddNewCompletedLevel(MapType.Beach);
            AddNewCompletedLevel(MapType.Forest);
        }
    }

    public void Save()
    {
        Data.Save();
    }

    public void SelectLevel(MapType mapType, int levelNumber)
    {
        SelectedLevelMap = new SelectedLevelMap { MapType = mapType, SelectedLevelIndex = levelNumber };
        Data.Get<GameData>().SelectedLevelMap.MapType = mapType;
        Data.Get<GameData>().SelectedLevelMap.SelectedLevelIndex = levelNumber;
        Save();
    }

    public void ModifyMoney(float amount)
    {
        Data.Get<GameData>().Coin += (int)amount;
        Coin = Data.Get<GameData>().Coin;
        OnCoinUpdated?.Invoke(Coin);
        Save();
    }

    public void AddDefaultUnlockedUnit(UnitHero unitHero)
    {
        if (UnlockedUnitList.Contains(unitHero)) return;

        Data.Get<GameData>().UnlockedUnitList.Add(unitHero);

        Save();
    }

    public void AddUnlockedUnit(UnitHero unitHero)
    {
        if (UnlockedUnitList.Contains(unitHero)) return;

        Data.Get<GameData>().UnlockedUnitList.Add(unitHero);
        SetNewPotatoStatus(true);

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

    public void SetSelectedLevel(MapType mapType, int levelIndex)
    {
        SelectedLevelMap = new SelectedLevelMap { MapType = mapType, SelectedLevelIndex = levelIndex };
        // Data.Get<GameData>().SelectedLevelMap.MapType = mapType;
        // Data.Get<GameData>().SelectedLevelMap.SelectedLevelIndex = levelIndex;
        Data.Get<GameData>().SelectedLevelMap = SelectedLevelMap;
        Save();
    }

    public void AddNewCompletedLevel(MapType mapType)
    {
        // Retrieve the GameData instance
        var gameData = Data.Get<GameData>();

        // Find the map or create a new one if it doesn't exist
        CompletedLevelMap completedLevelMap = gameData.CompletedLevelMapList.Find(i => i.MapType == mapType);
        if (completedLevelMap == null)
        {
            completedLevelMap = new CompletedLevelMap
            {
                MapType = mapType,
                CompletedLevelList = new List<int>(),
                HasCompletedAllLevels = false
            };

            // Add the new map to the list
            gameData.CompletedLevelMapList.Add(completedLevelMap);
        }

        // Save the updated data
        Save();
    }

    public void AddNewCompletedLevel(MapType mapType, int levelIndex, bool hasCompletedAllLevels)
    {
        // Retrieve the GameData instance
        var gameData = Data.Get<GameData>();

        // Find the map or create a new one if it doesn't exist
        CompletedLevelMap completedLevelMap = gameData.CompletedLevelMapList.Find(i => i.MapType == mapType);
        if (completedLevelMap == null)
        {
            completedLevelMap = new CompletedLevelMap
            {
                MapType = mapType,
                CompletedLevelList = new List<int>(),
                HasCompletedAllLevels = false
            };

            // Add the new map to the list
            gameData.CompletedLevelMapList.Add(completedLevelMap);
        }

        // Check if the level is already completed, if so, return early
        if (completedLevelMap.CompletedLevelList.Contains(levelIndex)) return;

        // Add the new completed level
        completedLevelMap.CompletedLevelList.Add(levelIndex);
        completedLevelMap.HasCompletedAllLevels = hasCompletedAllLevels;

        // Save the updated data
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

    public void SetCoinCollected(int amount)
    {
        CoinCollected = amount;
    }

    public void ClearCoinCollected()
    {
        CoinCollected = 0;
    }

    public void SetNewPotatoStatus(bool isActive)
    {
        var gameData = Data.Get<GameData>();
        gameData.IsThereNewPotato = isActive;
        IsThereNewPotato = isActive;

        Save();
    }
}
