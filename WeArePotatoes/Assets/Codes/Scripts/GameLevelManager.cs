using System.Collections;
using System.Collections.Generic;
using Farou.Utility;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameLevelManager : Singleton<GameLevelManager>
{
    private CoinManager coinManager;
    private LevelManager levelManager;
    private PauseManager pauseManager;
    private EnemyUnitSpawner enemyUnitSpawner;
    private PlayerUnitSpawner playerUnitSpawner;

    public new void Awake()
    {
        base.Awake();
        coinManager = GetComponent<CoinManager>();
        levelManager = GetComponent<LevelManager>();
        pauseManager = GetComponent<PauseManager>();
        enemyUnitSpawner = FindObjectOfType<EnemyUnitSpawner>();
        playerUnitSpawner = FindObjectOfType<PlayerUnitSpawner>();
    }

    private void Start()
    {

        enemyUnitSpawner.Initialize(levelManager.CurrentLevelWave);
        playerUnitSpawner.Initialize(GameDataManager.Instance.SelectedUnitList, GameDataManager.Instance.SeedProductionRate);
    }

    private void OnEnable()
    {
        EventManager.Subscribe(Farou.Utility.EventType.OnLevelWin, levelManager.HandleLevelWin);
        EventManager.Subscribe(Farou.Utility.EventType.OnLevelLose, levelManager.HandleLevelLose);
        EventManager<UnitData>.Subscribe(Farou.Utility.EventType.OnEnemyCoinDropped, HandleEnemyCoinDropped);
    }

    private void OnDisable()
    {
        EventManager.UnSubscribe(Farou.Utility.EventType.OnLevelWin, levelManager.HandleLevelWin);
        EventManager.UnSubscribe(Farou.Utility.EventType.OnLevelLose, levelManager.HandleLevelLose);
        EventManager<UnitData>.UnSubscribe(Farou.Utility.EventType.OnEnemyCoinDropped, HandleEnemyCoinDropped);
    }

    private void HandleEnemyCoinDropped(UnitData unitData)
    {
        coinManager.AddCoins(unitData.CoinReward);
    }
}

