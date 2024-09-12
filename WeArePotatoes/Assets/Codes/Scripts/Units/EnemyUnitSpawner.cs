using System;
using System.Collections;
using System.Collections.Generic;
using Farou.Utility;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyUnitSpawner : Singleton<EnemyUnitSpawner>
{
    [SerializeField] private string levelID;
    [SerializeField] private UnitDataSO unitDataSO;

    [SerializeField] private Transform baseTransform;
    [SerializeField] private Transform unitSpawnPoint;
    [SerializeField] private List<Unit> spawnedUnits = new List<Unit>();

    private LevelWaveSO levelWaveSO;

    public void Initialize(LevelWaveSO levelWaveSO)
    {
        this.levelWaveSO = levelWaveSO;
        StartCoroutine(SpawnUnitWaveRoutine());
    }

    private void OnEnable()
    {
        Unit.OnAnyUnitDead += EnemyUnit_OnAnyEnemyUnitDead;
    }

    private void OnDisable()
    {
        Unit.OnAnyUnitDead -= EnemyUnit_OnAnyEnemyUnitDead;
    }

    private IEnumerator SpawnUnitWaveRoutine()
    {
        // Cache levelWaveSO properties to reduce repeated access
        var delayAtStart = levelWaveSO.DelayAtStart;
        var delayBetweenWaves = levelWaveSO.DelayBetweenWaves;
        var waveDatas = levelWaveSO.WaveDatas;

        // Wait before starting the waves
        yield return new WaitForSeconds(delayAtStart);

        // Iterate through each wave
        for (int waveIndex = 0; waveIndex < waveDatas.Count; waveIndex++)
        {
            var currentWave = waveDatas[waveIndex];
            var waveUnitDatas = currentWave.WaveHeroDatas;

            // Iterate through each hero data in the wave
            for (int heroIndex = 0; heroIndex < waveUnitDatas.Count; heroIndex++)
            {
                var waveUnitData = waveUnitDatas[heroIndex];
                var unitType = waveUnitData.UnitType;
                UnitData unitData = unitDataSO.UnitStatDataList.Find(i => i.UnitHero == unitType);

                // Spawn the required number of units
                for (int i = 0; i < waveUnitData.Count; i++)
                {
                    SpawnUnit(unitType, unitData);

                    // Wait between waves
                    float delayBetweenUnitSpawn = delayBetweenWaves * 0.01f;
                    yield return new WaitForSeconds(delayBetweenUnitSpawn);
                }
            }

            // Wait between waves
            yield return new WaitForSeconds(delayBetweenWaves);
        }
    }


    private void EnemyUnit_OnAnyEnemyUnitDead(Unit unit)
    {
        if (unit && unit.UnitType == UnitType.Enemy)
        {
            spawnedUnits.Remove(unit);
            UnitObjectPool.Instance.ReturnToPool(unit.UnitData.UnitHero, unit);
            EventManager.Publish(Farou.Utility.EventType.OnEnemyCoinDropped);
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
        if (spawnedUnit)
        {
            spawnedUnit.transform.position = unitSpawnPoint.position + offset;
            float moveSpeed = unitDataSO.MoveSpeedDataList.Find(i => i.UnitMoveSpeedType == unitData.MoveSpeedType).MoveSpeed;
            float attackSpeed = unitDataSO.AttackSpeedDataList.Find(i => i.UnitAttackSpeedType == unitData.AttackSpeedType).AttackSpeed;
            spawnedUnit.InitializeUnit(UnitType.Enemy, unitData, baseTransform.position, moveSpeed, attackSpeed);
            spawnedUnits.Add(spawnedUnit);
        }
    }
}
