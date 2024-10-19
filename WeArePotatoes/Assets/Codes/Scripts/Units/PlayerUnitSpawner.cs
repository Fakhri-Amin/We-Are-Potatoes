using System;
using System.Collections;
using System.Collections.Generic;
using Farou.Utility;
using UnityEngine;
using UnityEngine.UI;

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

    public List<UnitHero> SelectedUnitTypeList => selectedUnitHeroList;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        ModifySeedCount(0);

        StartCoroutine(ProduceSeedRoutine());
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

    public void Initialize(List<UnitHero> selectedUnitHerolist, float seedProductionRate)
    {
        this.selectedUnitHeroList = selectedUnitHerolist;
        this.seedProductionRate = seedProductionRate;
    }

    private void HandleLevelEnd()
    {
        StopAllCoroutines();
    }

    private IEnumerator ProduceSeedRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(1 / seedProductionRate);

            ModifySeedCount(1);
        }
    }

    private void PlayerUnit_OnAnyPlayerUnitDead(Unit unit)
    {
        if (unit && unit.UnitType == UnitType.Player)
        {
            spawnedUnits.Remove(unit);
            // Destroy(unit.gameObject); // Consider pooling instead of destroying
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
        UnitData unitData = unitDataSO.UnitStatDataList.Find(i => i.UnitHero == unitHero);
        var unitSeedCost = unitData.SeedCost;

        if (SeedCount < unitSeedCost) return;

        Vector3 offset = new Vector3(0, UnityEngine.Random.Range(-0.5f, 0.5f), 0);
        Unit spawnedUnit = UnitObjectPool.Instance.GetPooledObject(unitHero);
        if (spawnedUnit)
        {
            ModifySeedCount(-unitSeedCost);

            spawnedUnit.transform.position = unitSpawnPoint.position + offset;

            UnitData newUnitData = unitData;

            float totalAttackDamage = GameDataManager.Instance.GetTotalAttackDamage();
            newUnitData.DamageAmount = unitData.DamageAmount;
            newUnitData.DamageAmount += unitData.DamageAmount * totalAttackDamage / 100;

            float totalUnitHealth = GameDataManager.Instance.GetTotalUnitHealth();
            newUnitData.Health = unitData.Health;
            newUnitData.Health += unitData.Health * totalUnitHealth / 100;

            float moveSpeed = unitDataSO.MoveSpeedDataList.Find(i => i.UnitMoveSpeedType == unitData.MoveSpeedType).MoveSpeed;
            float attackSpeed = unitDataSO.AttackSpeedDataList.Find(i => i.UnitAttackSpeedType == unitData.AttackSpeedType).AttackSpeed;
            spawnedUnit.InitializeUnit(UnitType.Player, newUnitData, baseTransform.position, moveSpeed, attackSpeed);
            spawnedUnits.Add(spawnedUnit);
        }
    }

    private void ModifySeedCount(float amount)
    {
        SeedCount += amount;
        OnSeedCountChanged?.Invoke(SeedCount);
    }
}
