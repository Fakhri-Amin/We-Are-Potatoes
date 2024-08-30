using System.Collections;
using System.Collections.Generic;
using Farou.Utility;
using MoreMountains.Feedbacks;
using UnityEngine;

public class EnemyUnitSpawner : Singleton<EnemyUnitSpawner>
{
    [SerializeField] private EnemyUnit enemyUnitPrefab;
    [SerializeField] private Transform enemyUnitSpawnPoint;
    [SerializeField] private MMFeedbacks unitDeadFeedbacks;

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
        if (unit && unit.UnitType == UnitType.Enemy)
        {
            Destroy(unit.gameObject);
        }
    }

    private void OnSpawn()
    {
        Vector3 offset = new Vector3(0, Random.Range(-0.5f, 0.5f), 0);
        EnemyUnit unit = Instantiate(enemyUnitPrefab, enemyUnitSpawnPoint.position + offset, Quaternion.identity);
    }

}
