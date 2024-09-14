using System.Collections;
using System.Collections.Generic;
using Farou.Utility;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameLevelManager : MonoBehaviour
{
    public static GameLevelManager Instance { get; private set; }
    private CoinManager coinManager;
    private LevelManager levelManager;
    private PauseManager pauseManager;
    private EnemyUnitSpawner enemyUnitSpawner;
    private PlayerUnitSpawner playerUnitSpawner;

    private void Awake()
    {
        Instance = this;

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

        coinManager.UpdateCoinUI();

        EventManager.Subscribe(Farou.Utility.EventType.OnLevelWin, () => StartCoroutine(levelManager.HandleLevelWin()));
        EventManager.Subscribe(Farou.Utility.EventType.OnLevelLose, () => StartCoroutine(levelManager.HandleLevelLose()));
        EventManager<UnitData>.Subscribe(Farou.Utility.EventType.OnEnemyCoinDropped, HandleEnemyCoinDropped);
    }

    private void OnEnable()
    {
    }

    private void OnDisable()
    {
        EventManager.UnSubscribe(Farou.Utility.EventType.OnLevelWin, () => StartCoroutine(levelManager.HandleLevelWin()));
        EventManager.UnSubscribe(Farou.Utility.EventType.OnLevelLose, () => StartCoroutine(levelManager.HandleLevelLose()));
        EventManager<UnitData>.UnSubscribe(Farou.Utility.EventType.OnEnemyCoinDropped, HandleEnemyCoinDropped);
    }

    private void HandleEnemyCoinDropped(UnitData unitData)
    {
        coinManager.AddCoins(unitData.CoinReward);
    }
}

