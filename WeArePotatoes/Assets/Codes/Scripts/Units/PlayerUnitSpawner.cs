using System;
using System.Collections;
using System.Collections.Generic;
using Farou.Utility;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUnitSpawner : Singleton<PlayerUnitSpawner>
{
    public event Action<float> OnSeedCountChanged;
    public float SeedCount = 0;
    [SerializeField] private UnitDataSO unitDataSO;
    [SerializeField] private Transform baseTransform;
    [SerializeField] private Transform unitSpawnPoint;
    [SerializeField] private List<Unit> spawnedUnits = new List<Unit>();

    private void Start()
    {
        ModifySeedCount(0);

        StartCoroutine(ProduceSeedRoutine());
    }

    private void OnEnable()
    {
        Unit.OnAnyUnitDead += PlayerUnit_OnAnyPlayerUnitDead;
    }

    private void OnDisable()
    {
        Unit.OnAnyUnitDead -= PlayerUnit_OnAnyPlayerUnitDead;
    }

    private IEnumerator ProduceSeedRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);

            ModifySeedCount(1);
        }
    }

    private void PlayerUnit_OnAnyPlayerUnitDead(Unit unit)
    {
        if (unit && unit.UnitType == UnitType.Player)
        {
            spawnedUnits.Remove(unit);
            // Destroy(unit.gameObject); // Consider pooling instead of destroying
            UnitObjectPool.Instance.ReturnToPool(unit.Stat.UnitHero, unit);
        }
    }

    public Vector3 GetUnitPosition(Unit unit)
    {
        Unit foundUnit = spawnedUnits.Find(i => i == unit);
        return foundUnit ? foundUnit.transform.position : Vector3.zero;
    }

    public void OnUnitSpawn(UnitHero unitHero)
    {
        var unitSeedCost = unitDataSO.UnitStatDataList.Find(i => i.UnitHero == unitHero).SeedCost;

        if (SeedCount < unitSeedCost) return;

        Vector3 offset = new Vector3(0, UnityEngine.Random.Range(-0.5f, 0.5f), 0);
        Unit spawnedUnit = UnitObjectPool.Instance.GetPooledObject(unitHero);
        if (spawnedUnit)
        {
            ModifySeedCount(-unitSeedCost);

            spawnedUnit.transform.position = unitSpawnPoint.position + offset;
            spawnedUnit.InitializeUnit(UnitType.Player, baseTransform.position);
            spawnedUnits.Add(spawnedUnit);
        }
    }

    private void ModifySeedCount(float amount)
    {
        SeedCount += amount;
        OnSeedCountChanged?.Invoke(SeedCount);
    }
}
