using System.Collections.Generic;
using Farou.Utility;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUnitSpawner : Singleton<PlayerUnitSpawner>
{
    [SerializeField] private Transform baseTransform;
    [SerializeField] private Transform unitSpawnPoint;
    [SerializeField] private Button[] unitButtons; // Array to simplify button handling
    [SerializeField] private List<Unit> spawnedUnits = new List<Unit>();

    private new void Awake()
    {
        base.Awake();
        for (int i = 0; i < unitButtons.Length; i++)
        {
            int buttonIndex = i; // Capture the button index

            for (int j = 0; j < UnitObjectPool.Instance.UnitHeroReferences.Count; j++)
            {
                if (buttonIndex == j) // Compare with captured button index
                {
                    var unitHeroReference = UnitObjectPool.Instance.UnitHeroReferences[j]; // Capture reference here

                    unitButtons[buttonIndex].onClick.AddListener(() => OnUnitSpawn(unitHeroReference.Type));
                }
            }
        }

    }

    private void OnEnable()
    {
        Unit.OnAnyUnitDead += PlayerUnit_OnAnyPlayerUnitDead;
    }

    private void OnDisable()
    {
        Unit.OnAnyUnitDead -= PlayerUnit_OnAnyPlayerUnitDead;
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

    private void OnUnitSpawn(UnitHero unitHero)
    {
        Vector3 offset = new Vector3(0, Random.Range(-0.5f, 0.5f), 0);
        Unit spawnedUnit = UnitObjectPool.Instance.GetPooledObject(unitHero);
        if (spawnedUnit)
        {
            spawnedUnit.transform.position = unitSpawnPoint.position + offset;
            spawnedUnit.InitializeUnit(UnitType.Player, baseTransform.position);
            spawnedUnits.Add(spawnedUnit);
        }
    }
}