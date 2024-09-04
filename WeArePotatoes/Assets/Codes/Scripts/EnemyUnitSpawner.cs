using System.Collections;
using System.Collections.Generic;
using Farou.Utility;
using MoreMountains.Feedbacks;
using UnityEngine;

public class EnemyUnitSpawner : Singleton<EnemyUnitSpawner>
{
    [SerializeField] private Transform baseTransform;
    [SerializeField] private Unit unitSword;
    [SerializeField] private Unit unitBow;
    [SerializeField] private Unit unitCatapult;
    [SerializeField] private Transform enemyUnitSpawnPoint;
    [SerializeField] private MMFeedbacks unitDeadFeedbacks;
    [SerializeField] private ParticleSystem unitDeadVFX;
    [SerializeField] private ParticleSystem projectileHitVFX;
    [SerializeField] private List<Unit> spawnedUnits = new List<Unit>();

    private void OnEnable()
    {
        Unit.OnAnyUnitDead += EnemyUnit_OnAnyEnemyUnitDead;
        Projectile.OnProjectileAreaHit += EnemyUnit_OnProjectileAreaHit;
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
        else if (Input.GetKeyDown(KeyCode.C))
        {
            OnUnitCatapultSpawn();
        }
    }

    private void EnemyUnit_OnAnyEnemyUnitDead(Unit unit)
    {
        if (unit && unit.UnitType == UnitType.Enemy)
        {
            var vfx = Instantiate(unitDeadVFX, unit.transform.position, Quaternion.identity);
            vfx.gameObject.SetActive(true);

            spawnedUnits.Remove(unit);
            Destroy(unit.gameObject);
        }
    }

    private void EnemyUnit_OnProjectileAreaHit(Projectile projectile)
    {
        var vfx = Instantiate(projectileHitVFX, projectile.transform.position, Quaternion.identity);
        vfx.gameObject.SetActive(true);
    }


    private void OnUnitBowSpawn()
    {
        OnUnitSpawn(unitBow);
    }

    private void OnUnitSwordSpawn()
    {
        OnUnitSpawn(unitSword);
    }

    private void OnUnitCatapultSpawn()
    {
        OnUnitSpawn(unitCatapult);
    }

    public Vector3 GetUnitPosition(Unit unit) => spawnedUnits.Find(i => i == unit).gameObject.transform.position;

    private void OnUnitSpawn(Unit unit)
    {
        Vector3 offset = new Vector3(0, Random.Range(-0.5f, 0.5f), 0);
        Unit spawnedUnit = Instantiate(unit, enemyUnitSpawnPoint.position + offset, Quaternion.identity);
        spawnedUnit.InitializeUnit(UnitType.Enemy, baseTransform.position);
        spawnedUnits.Add(spawnedUnit);
    }

}
