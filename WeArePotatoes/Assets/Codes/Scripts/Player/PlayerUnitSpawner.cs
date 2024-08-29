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
        Vector3 offset = new Vector3(0, Random.Range(-0.5f, 0.5f), 0);
        PlayerUnit unit = Instantiate(playerUnitPrefab, playerUnitSpawnPoint.position + offset, Quaternion.identity);
    }

}
