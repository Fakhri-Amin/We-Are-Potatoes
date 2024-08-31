using System.Collections;
using System.Collections.Generic;
using Farou.Utility;
using MoreMountains.Feedbacks;
using UnityEngine;

public class EnemyUnitSpawner : Singleton<EnemyUnitSpawner>
{
    [SerializeField] private Unit unitSword;
    [SerializeField] private Unit unitBow;
    [SerializeField] private Transform enemyUnitSpawnPoint;
    [SerializeField] private MMFeedbacks unitDeadFeedbacks;

    private new void Awake()
    {
        Unit.OnAnyUnitDead += EnemyUnit_OnAnyEnemyUnitDead;
    }

    private void Start()
    {

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            OnUnitSwordSpawn();
        }
        else if (Input.GetKeyDown(KeyCode.X))
        {
            OnUnitBowSpawn();
        }
    }

    private void EnemyUnit_OnAnyEnemyUnitDead(Unit unit)
    {
        if (unit && unit.UnitType == UnitType.Enemy)
        {
            Destroy(unit.gameObject);
        }
    }

    private void OnUnitBowSpawn()
    {
        OnUnitSpawn(unitBow);
    }

    private void OnUnitSwordSpawn()
    {
        OnUnitSpawn(unitSword);
    }

    private void OnUnitSpawn(Unit unit)
    {
        Vector3 offset = new Vector3(0, Random.Range(-0.5f, 0.5f), 0);
        Unit spawnedUnit = Instantiate(unit, enemyUnitSpawnPoint.position + offset, Quaternion.identity);
        spawnedUnit.InitializeUnit(UnitType.Enemy);
    }

}
