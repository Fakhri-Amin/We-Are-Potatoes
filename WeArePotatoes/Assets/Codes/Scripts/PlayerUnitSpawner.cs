using System.Collections;
using System.Collections.Generic;
using Farou.Utility;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUnitSpawner : Singleton<PlayerUnitSpawner>
{
    [SerializeField] private List<PlayerUnit> playerUnits = new List<PlayerUnit>();
    [SerializeField] private PlayerUnit playerUnitPrefab;
    [SerializeField] private Transform playerUnitSpawnPoint;
    [SerializeField] private Button spawnButton;

    private new void Awake()
    {
        base.Awake();
        spawnButton.onClick.AddListener(OnSpawn);
    }

    private void OnSpawn()
    {
        PlayerUnit unit = Instantiate(playerUnitPrefab, playerUnitSpawnPoint.position, Quaternion.identity);
        playerUnits.Add(unit);
    }

    public List<PlayerUnit> GetPlayerUnits() => playerUnits;
}
