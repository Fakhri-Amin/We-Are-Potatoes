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

    public new void Awake()
    {
        base.Awake();
        coinManager = GetComponent<CoinManager>();
        levelManager = GetComponent<LevelManager>();
        pauseManager = GetComponent<PauseManager>();
        enemyUnitSpawner = FindObjectOfType<EnemyUnitSpawner>();
    }

    private void Start()
    {
        enemyUnitSpawner.Initialize(levelManager.CurrentLevelWave);
    }

    private void OnEnable()
    {
        EventManager.Subscribe(Farou.Utility.EventType.OnLevelWin, levelManager.HandleLevelWin);
        EventManager.Subscribe(Farou.Utility.EventType.OnLevelLose, levelManager.HandleLevelLose);
        EventManager.Subscribe(Farou.Utility.EventType.OnEnemyCoinDropped, HandleEnemyCoinDropped);
    }

    private void OnDisable()
    {
        EventManager.UnSubscribe(Farou.Utility.EventType.OnLevelWin, levelManager.HandleLevelWin);
        EventManager.UnSubscribe(Farou.Utility.EventType.OnLevelLose, levelManager.HandleLevelLose);
        EventManager.UnSubscribe(Farou.Utility.EventType.OnEnemyCoinDropped, HandleEnemyCoinDropped);
    }

    private void HandleEnemyCoinDropped()
    {
        coinManager.AddCoins(100);
    }
}
