using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUnitSpawner : MonoBehaviour
{
    [SerializeField] private GameObject playerUnitPrefab;
    [SerializeField] private Transform playerUnitSpawnPoint;
    [SerializeField] private Button spawnButton;

    private void Awake()
    {
        spawnButton.onClick.AddListener(OnSpawn);
    }

    private void OnSpawn()
    {
        Instantiate(playerUnitPrefab, playerUnitSpawnPoint.position, Quaternion.identity);
    }
}
