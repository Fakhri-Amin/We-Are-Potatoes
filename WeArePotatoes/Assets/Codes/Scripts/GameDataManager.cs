using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Farou.Utility;

[DefaultExecutionOrder(-99999999)]
public class GameDataManager : PersistentSingleton<GameDataManager>
{
    public event Action<int> OnCoinUpdated;
    public event Action<List<UnitHero>> OnSelectedUnitListChanged;
    public event Action<float> OnSeedProductionRateChanged;
    public event Action<int> OnBaseHealthChanged;

    public UnitDataSO UnitDataSO;
    public LevelWaveDatabaseSO LevelWaveDatabaseSO;
    public BaseBuildingSO BaseBuildingSO;
    public List<UnitHero> SelectedUnitList = new List<UnitHero>(3);
    public List<UnitHero> UnlockedUnitList = new List<UnitHero>();
    public List<int> CompletedLevelList = new List<int>();
    public int SelectedLevelIndex = 0;
    public float SeedProductionRate;
    public int BaseHealth;

    public new void Awake()
    {
        base.Awake();

        int coin = Data.Get<GameData>().Coin;
        OnCoinUpdated?.Invoke(coin);

        SelectedUnitList = Data.Get<GameData>().SelectedUnitList;
        OnSelectedUnitListChanged?.Invoke(SelectedUnitList);

        UnlockedUnitList = Data.Get<GameData>().UnlockedUnitList;
        CompletedLevelList = Data.Get<GameData>().CompletedLevelList;

        SeedProductionRate = Data.Get<GameData>().SeedProductionRate;
        OnSeedProductionRateChanged?.Invoke(SeedProductionRate);

        BaseHealth = Data.Get<GameData>().BaseHealth;
        OnBaseHealthChanged?.Invoke(BaseHealth);
    }

    private void Start()
    {
        // Set default data
        AddNewCompletedLevel(0);

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

        if (SeedProductionRate == 0)
        {
            SetNewSeedProductionRate(BaseBuildingSO.SeedProductionRate);
        }

        if (BaseHealth == 0)
        {
            SetNewBaseHealth(BaseBuildingSO.BaseHealth);
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

    public void ModifyMoney(int amount)
    {
        Data.Get<GameData>().Coin += amount;
        OnCoinUpdated?.Invoke(Data.Get<GameData>().Coin);
        Save();
    }

    public void AddUnlockedUnit(UnitHero unitHero)
    {
        if (UnlockedUnitList.Contains(unitHero)) return;

        Data.Get<GameData>().UnlockedUnitList.Add(unitHero);
        Save();
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

    public void SetNewSeedProductionRate(float rate)
    {
        SeedProductionRate = rate;
        Data.Get<GameData>().SeedProductionRate = rate;
        Save();

        OnSeedProductionRateChanged?.Invoke(SeedProductionRate);
    }

    public void SetNewBaseHealth(int health)
    {
        BaseHealth = health;
        Data.Get<GameData>().BaseHealth = health;
        Save();

        OnBaseHealthChanged?.Invoke(BaseHealth);
    }
}
