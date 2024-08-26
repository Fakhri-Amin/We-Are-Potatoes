using System;
using System.Collections;
using System.Collections.Generic;
using Farou.Utility;
using UnityEngine;

public class EnemyUnitSpawner : Singleton<EnemyUnitSpawner>
{
    [SerializeField] private List<EnemyUnit> enemyUnits = new List<EnemyUnit>();
    [SerializeField] private EnemyUnit enemyUnitPrefab;
    [SerializeField] private Transform enemyUnitSpawnPoint;

    private new void Awake()
    {
        EnemyUnit.OnAnyEnemyUnitDead += EnemyUnit_OnAnyEnemyUnitDead;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            OnSpawn();
        }
    }

    private void EnemyUnit_OnAnyEnemyUnitDead(EnemyUnit unit)
    {
        enemyUnits.Remove(unit);
        Destroy(unit.gameObject);
    }

    private void OnSpawn()
    {
        EnemyUnit unit = Instantiate(enemyUnitPrefab, enemyUnitSpawnPoint.position, Quaternion.identity);
        enemyUnits.Add(unit);
    }

    public List<EnemyUnit> GetEnemyUnits() => enemyUnits;
}
