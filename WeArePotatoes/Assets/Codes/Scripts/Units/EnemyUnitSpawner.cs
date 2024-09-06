using System.Collections.Generic;
using Farou.Utility;
using MoreMountains.Feedbacks;
using UnityEngine;

public class EnemyUnitSpawner : Singleton<EnemyUnitSpawner>
{
    [SerializeField] private Transform baseTransform;
    [SerializeField] private Transform unitSpawnPoint;
    [SerializeField] private List<Unit> spawnedUnits = new List<Unit>();

    private void OnEnable()
    {
        Unit.OnAnyUnitDead += EnemyUnit_OnAnyEnemyUnitDead;
        Unit.OnAnyUnitDead += EnemyUnit_OnAnyEnemyUnitHit;
    }

    private void OnDisable()
    {
        Unit.OnAnyUnitDead -= EnemyUnit_OnAnyEnemyUnitDead;
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
    }

    private void EnemyUnit_OnAnyEnemyUnitHit(Unit unit)
    {
        if (unit && unit.UnitType == UnitType.Enemy)
        {
            spawnedUnits.Remove(unit);
            // Destroy(unit.gameObject); // Consider object pooling here for better performance
            UnitObjectPool.Instance.ReturnToPool(unit.Stat.UnitHero, unit);
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
