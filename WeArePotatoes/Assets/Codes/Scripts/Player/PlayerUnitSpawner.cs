using System.Collections;
using System.Collections.Generic;
using Farou.Utility;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUnitSpawner : Singleton<PlayerUnitSpawner>
{
    [SerializeField] private Transform baseTransform;
    [SerializeField] private Unit unitSword;
    [SerializeField] private Unit unitBow;
    [SerializeField] private Unit unitCatapult;
    [SerializeField] private Transform playerUnitSpawnPoint;
    [SerializeField] private Button unitSwordButton;
    [SerializeField] private Button unitBowButton;
    [SerializeField] private Button unitCatapultButton;

    private new void Awake()
    {
        base.Awake();
        unitSwordButton.onClick.AddListener(OnUnitSwordSpawn);
        unitBowButton.onClick.AddListener(OnUnitBowSpawn);
        unitCatapultButton.onClick.AddListener(OnUnitCatapultSpawn);
    }

    private void OnEnable()
    {
        Unit.OnAnyUnitDead += PlayerUnit_OnAnyPlayerUnitDead;
    }

    private void PlayerUnit_OnAnyPlayerUnitDead(Unit unit)
    {
        if (unit && unit.UnitType == UnitType.Player) Destroy(unit.gameObject);
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

    private void OnUnitSpawn(Unit unit)
    {
        Vector3 offset = new Vector3(0, Random.Range(-0.5f, 0.5f), 0);
        Unit spawnedUnit = Instantiate(unit, playerUnitSpawnPoint.position + offset, Quaternion.identity);
        spawnedUnit.InitializeUnit(UnitType.Player, baseTransform.position);
    }
}
