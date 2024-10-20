using System;
using System.Collections;
using System.Collections.Generic;
using Farou.Utility;
using UnityEngine;

public class EnemyUnitSpawner : MonoBehaviour
{
    public static EnemyUnitSpawner Instance { get; private set; }
    [SerializeField] private UnitDataSO unitDataSO;

    [SerializeField] private Transform baseTransform;
    [SerializeField] private Transform unitSpawnPoint;
    [SerializeField] private List<Unit> spawnedUnits = new List<Unit>();

    private LevelWaveSO levelWaveSO;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject); // Avoid duplicate instances
        }
    }

    public void Initialize(LevelWaveSO levelWaveSO)
    {
        this.levelWaveSO = levelWaveSO;
        StartCoroutine(SpawnUnitWaveRoutine());
    }

    private void OnEnable()
    {
        Unit.OnAnyUnitDead += EnemyUnit_OnAnyEnemyUnitDead;
        EventManager.Subscribe(Farou.Utility.EventType.OnLevelWin, HandleLevelEnd);
        EventManager.Subscribe(Farou.Utility.EventType.OnLevelLose, HandleLevelEnd);
    }

    private void OnDisable()
    {
        Unit.OnAnyUnitDead -= EnemyUnit_OnAnyEnemyUnitDead;
        EventManager.UnSubscribe(Farou.Utility.EventType.OnLevelWin, HandleLevelEnd);
        EventManager.UnSubscribe(Farou.Utility.EventType.OnLevelLose, HandleLevelEnd);
    }

    private void OnDestroy()
    {
        Unit.OnAnyUnitDead -= EnemyUnit_OnAnyEnemyUnitDead;
        EventManager.UnSubscribe(Farou.Utility.EventType.OnLevelWin, HandleLevelEnd);
        EventManager.UnSubscribe(Farou.Utility.EventType.OnLevelLose, HandleLevelEnd);
    }

    private void HandleLevelEnd()
    {
        StopAllCoroutines();
    }

    private IEnumerator SpawnUnitWaveRoutine()
    {
        float delayAtStart = levelWaveSO.DelayAtStart;
        float delayBetweenWaves = levelWaveSO.DelayBetweenWaves;
        List<WaveData> waveDatas = levelWaveSO.WaveDatas;

        yield return new WaitForSeconds(delayAtStart); // Initial delay before waves

        // Iterate through all waves
        foreach (WaveData wave in waveDatas)
        {
            yield return StartCoroutine(SpawnUnitsForWave(wave));
            yield return new WaitForSeconds(delayBetweenWaves); // Delay between waves
        }
    }

    private IEnumerator SpawnUnitsForWave(WaveData waveData)
    {
        List<WaveHeroData> waveUnitDatas = waveData.WaveHeroDatas;
        float delayBetweenUnitSpawn = levelWaveSO.DelayBetweenWaves * 0.01f;

        foreach (WaveHeroData unitData in waveUnitDatas)
        {
            UnitData unitStatData = unitDataSO.UnitStatDataList.Find(i => i.UnitHero == unitData.UnitType);
            if (unitStatData == null)
            {
                Debug.LogWarning("Unit data not found for hero: " + unitData.UnitType);
                continue;
            }

            for (int i = 0; i < unitData.Count; i++)
            {
                SpawnUnit(unitData.UnitType, unitStatData);
                yield return new WaitForSeconds(delayBetweenUnitSpawn);
            }
        }
    }

    private void EnemyUnit_OnAnyEnemyUnitDead(Unit unit)
    {
        if (unit && unit.UnitType == UnitType.Enemy)
        {
            EventManager<UnitData>.Publish(Farou.Utility.EventType.OnEnemyCoinDropped, unit.UnitData);
            spawnedUnits.Remove(unit);
            UnitObjectPool.Instance.ReturnToPool(unit.UnitData.UnitHero, unit);
        }
    }

    public Vector3 GetUnitPosition(Unit unit)
    {
        Unit foundUnit = spawnedUnits.Find(i => i == unit);
        return foundUnit ? foundUnit.transform.position : Vector3.zero;
    }

    private void SpawnUnit(UnitHero unitHero, UnitData unitData)
    {
        Vector3 offset = new Vector3(0, UnityEngine.Random.Range(-0.5f, 0.5f), 0);
        Unit spawnedUnit = UnitObjectPool.Instance.GetPooledObject(unitHero);

        if (spawnedUnit == null)
        {
            Debug.LogWarning("No available pooled object for unit: " + unitHero);
            return;
        }

        // Fetch and initialize unit stats
        unitData.DamageAmount = unitDataSO.UnitStatDataList.Find(i => i.UnitHero == unitHero).DamageAmount;
        unitData.Health = unitDataSO.UnitStatDataList.Find(i => i.UnitHero == unitHero).Health;

        float moveSpeed = unitDataSO.MoveSpeedDataList.Find(i => i.UnitMoveSpeedType == unitData.MoveSpeedType).MoveSpeed;
        float attackSpeed = unitDataSO.AttackSpeedDataList.Find(i => i.UnitAttackSpeedType == unitData.AttackSpeedType).AttackSpeed;

        // Initialize unit with specific stats and position
        spawnedUnit.transform.position = unitSpawnPoint.position + offset;
        spawnedUnit.InitializeUnit(UnitType.Enemy, unitData, baseTransform.position,
            0 /* attackDamageBoost */, 0 /* unitHealthBoost */, moveSpeed, attackSpeed);

        spawnedUnits.Add(spawnedUnit);
    }
}
