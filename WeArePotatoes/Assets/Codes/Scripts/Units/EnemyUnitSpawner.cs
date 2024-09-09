using System.Collections;
using System.Collections.Generic;
using Farou.Utility;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyUnitSpawner : Singleton<EnemyUnitSpawner>
{
    [SerializeField] private string levelID;
    [SerializeField] private LevelWaveDatabaseSO levelWaveDatabaseSO;
    [SerializeField] private Transform baseTransform;
    [SerializeField] private Transform unitSpawnPoint;
    [SerializeField] private List<Unit> spawnedUnits = new List<Unit>();

    private LevelWaveSO levelWaveSO;

    private void Start()
    {
        levelID = SceneManager.GetActiveScene().name;

        foreach (var item in levelWaveDatabaseSO.LevelWaveSOs)
        {
            if (item.name == levelID)
            {
                levelWaveSO = item;
            }
        }

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
        yield return new WaitForSeconds(levelWaveSO.DelayAtStart);

        foreach (var wave in levelWaveSO.WaveDatas)
        {
            foreach (var unit in wave.WaveHeroDatas)
            {
                for (int i = 0; i < unit.Count; i++)
                {
                    SpawnUnit(unit.UnitType);
                }
            }

            yield return new WaitForSeconds(levelWaveSO.DelayBetweenWaves);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            SpawnUnit(UnitHero.Sword);
        }
        else if (Input.GetKeyDown(KeyCode.X))
        {
            SpawnUnit(UnitHero.Bow);
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            SpawnUnit(UnitHero.Catapult);
        }
        else if (Input.GetKeyDown(KeyCode.V))
        {
            SpawnUnit(UnitHero.Shield);
        }
        else if (Input.GetKeyDown(KeyCode.B))
        {
            SpawnUnit(UnitHero.Axe);
        }
        else if (Input.GetKeyDown(KeyCode.N))
        {
            SpawnUnit(UnitHero.Sniper);
        }
        else if (Input.GetKeyDown(KeyCode.M))
        {
            SpawnUnit(UnitHero.Sniper);
        }
        else if (Input.GetKeyDown(KeyCode.K))
        {
            SpawnUnit(UnitHero.GreatSword);
        }
    }

    private void EnemyUnit_OnAnyEnemyUnitDead(Unit unit)
    {
        if (unit && unit.UnitType == UnitType.Enemy)
        {
            spawnedUnits.Remove(unit);
            // Destroy(unit.gameObject); // Consider object pooling here for better performance
            UnitObjectPool.Instance.ReturnToPool(unit.Stat.UnitHero, unit);
        }
    }

    public Vector3 GetUnitPosition(Unit unit)
    {
        Unit foundUnit = spawnedUnits.Find(i => i == unit);
        return foundUnit ? foundUnit.transform.position : Vector3.zero;
    }

    private void SpawnUnit(UnitHero unitHero)
    {
        Vector3 offset = new Vector3(0, Random.Range(-0.5f, 0.5f), 0);
        Unit spawnedUnit = UnitObjectPool.Instance.GetPooledObject(unitHero);
        if (spawnedUnit)
        {
            spawnedUnit.transform.position = unitSpawnPoint.position + offset;
            spawnedUnit.InitializeUnit(UnitType.Enemy, baseTransform.position);
            spawnedUnits.Add(spawnedUnit);
        }
    }
}
