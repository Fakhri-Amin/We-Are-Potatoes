using System;
using System.Collections;
using System.Collections.Generic;
using Farou.Utility;
using UnityEngine;

public class EnemyUnitSpawner : Singleton<EnemyUnitSpawner>
{
    [SerializeField] private EnemyUnit enemyUnitPrefab;
    [SerializeField] private Transform enemyUnitSpawnPoint;

    private new void Awake()
    {
        Unit.OnAnyUnitDead += EnemyUnit_OnAnyEnemyUnitDead;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            OnSpawn();
        }
    }

    private void EnemyUnit_OnAnyEnemyUnitDead(Unit unit)
    {
        if (unit && unit.UnitType == UnitType.Enemy) Destroy(unit.gameObject);
    }

    private void OnSpawn()
    {
        EnemyUnit unit = Instantiate(enemyUnitPrefab, enemyUnitSpawnPoint.position, Quaternion.identity);
    }

}
