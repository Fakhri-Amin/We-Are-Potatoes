using System.Collections;
using System.Collections.Generic;
using Farou.Utility;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUnitSpawner : Singleton<PlayerUnitSpawner>
{
    [SerializeField] private PlayerUnit playerUnitPrefab;
    [SerializeField] private Transform playerUnitSpawnPoint;
    [SerializeField] private Button spawnButton;

    private new void Awake()
    {
        base.Awake();
        spawnButton.onClick.AddListener(OnSpawn);
    }

    private void OnEnable()
    {
        Unit.OnAnyUnitDead += PlayerUnit_OnAnyPlayerUnitDead;
    }

    private void PlayerUnit_OnAnyPlayerUnitDead(Unit unit)
    {
        if (unit && unit.UnitType == UnitType.Player) Destroy(unit.gameObject);
    }

    private void OnSpawn()
    {
        PlayerUnit unit = Instantiate(playerUnitPrefab, playerUnitSpawnPoint.position, Quaternion.identity);
    }

}
