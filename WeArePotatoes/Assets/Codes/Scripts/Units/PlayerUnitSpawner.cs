using System;
using System.Collections;
using System.Collections.Generic;
using Farou.Utility;
using UnityEngine;

public class PlayerUnitSpawner : MonoBehaviour
{
    public static PlayerUnitSpawner Instance { get; private set; }
    public event Action<float> OnSeedCountChanged;
    public float SeedCount = 0;

    [SerializeField] private UnitDataSO unitDataSO;
    [SerializeField] private Transform baseTransform;
    [SerializeField] private Transform unitSpawnPoint;
    [SerializeField] private List<Unit> spawnedUnits = new List<Unit>();

    private List<UnitHero> selectedUnitHeroList = new List<UnitHero>();
    private float seedProductionRate;
    private Coroutine seedProductionCoroutine;

    public List<UnitHero> SelectedUnitTypeList => selectedUnitHeroList;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        ModifySeedCount(0);
        seedProductionCoroutine = StartCoroutine(ProduceSeedRoutine());
    }

    private void OnEnable()
    {
        Unit.OnAnyUnitDead += PlayerUnit_OnAnyPlayerUnitDead;
        EventManager.Subscribe(Farou.Utility.EventType.OnLevelWin, HandleLevelEnd);
        EventManager.Subscribe(Farou.Utility.EventType.OnLevelLose, HandleLevelEnd);
    }

    private void OnDisable()
    {
        Unit.OnAnyUnitDead -= PlayerUnit_OnAnyPlayerUnitDead;
        EventManager.UnSubscribe(Farou.Utility.EventType.OnLevelWin, HandleLevelEnd);
        EventManager.UnSubscribe(Farou.Utility.EventType.OnLevelLose, HandleLevelEnd);
    }

    private void OnDestroy()
    {
        Unit.OnAnyUnitDead -= PlayerUnit_OnAnyPlayerUnitDead;
        EventManager.UnSubscribe(Farou.Utility.EventType.OnLevelWin, HandleLevelEnd);
        EventManager.UnSubscribe(Farou.Utility.EventType.OnLevelLose, HandleLevelEnd);
    }

    public void Initialize(List<UnitHero> selectedUnitHerolist, float seedProductionRate)
    {
        this.selectedUnitHeroList = selectedUnitHerolist;
        this.seedProductionRate = seedProductionRate;
    }

    private void HandleLevelEnd()
    {
        if (seedProductionCoroutine != null)
        {
            StopCoroutine(seedProductionCoroutine);
            seedProductionCoroutine = null;
        }
    }

    private IEnumerator ProduceSeedRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(1 / seedProductionRate);
            ProduceSeeds();
        }
    }

    private void ProduceSeeds()
    {
        ModifySeedCount(1);
    }

    private void PlayerUnit_OnAnyPlayerUnitDead(Unit unit)
    {
        if (unit && unit.UnitType == UnitType.Player)
        {
            spawnedUnits.Remove(unit);
            unit.ResetState(); // Reset unit state before returning it to the pool
            UnitObjectPool.Instance.ReturnToPool(unit.UnitData.UnitHero, unit);
        }
    }

    public Vector3 GetUnitPosition(Unit unit)
    {
        Unit foundUnit = spawnedUnits.Find(i => i == unit);
        return foundUnit ? foundUnit.transform.position : Vector3.zero;
    }

    public void OnUnitSpawn(UnitHero unitHero)
    {
        UnitData unitData = unitDataSO?.UnitStatDataList.Find(i => i.UnitHero == unitHero);
        if (unitData == null)
        {
            Debug.LogWarning("Unit data not found for unitHero: " + unitHero);
            return;
        }

        var unitSeedCost = unitData.SeedCost;
        if (SeedCount < unitSeedCost) return;

        SpawnUnit(unitData);
    }

    private void SpawnUnit(UnitData unitData)
    {
        Vector3 offset = new Vector3(0, UnityEngine.Random.Range(-0.5f, 0.5f), 0);
        Unit spawnedUnit = UnitObjectPool.Instance.GetPooledObject(unitData.UnitHero);
        if (spawnedUnit == null)
        {
            Debug.LogWarning("No available pooled object for unit: " + unitData.UnitHero);
            return;
        }

        ModifySeedCount(-unitData.SeedCost);
        spawnedUnit.transform.position = unitSpawnPoint.position + offset;

        InitializeSpawnedUnit(spawnedUnit, unitData);
        spawnedUnits.Add(spawnedUnit);
    }

    private void InitializeSpawnedUnit(Unit unit, UnitData unitData)
    {
        float totalAttackDamage = GameDataManager.Instance.GetTotalAttackDamagePercentage();
        float attackDamageBoost = unitData.DamageAmount * totalAttackDamage / 100;

        float totalUnitHealth = GameDataManager.Instance.GetTotalUnitHealthPercentage();
        float unitHealthBoost = unitData.Health * totalUnitHealth / 100;

        float moveSpeed = unitDataSO.MoveSpeedDataList
            .Find(i => i.UnitMoveSpeedType == unitData.MoveSpeedType).MoveSpeed;
        float attackSpeed = unitDataSO.AttackSpeedDataList
            .Find(i => i.UnitAttackSpeedType == unitData.AttackSpeedType).AttackSpeed;

        unit.InitializeUnit(UnitType.Player, unitData, baseTransform.position,
            attackDamageBoost, unitHealthBoost, moveSpeed, attackSpeed);
    }

    private void ModifySeedCount(float amount)
    {
        SeedCount += amount;
        OnSeedCountChanged?.Invoke(SeedCount);
    }
}
