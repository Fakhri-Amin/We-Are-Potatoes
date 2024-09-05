using System.Collections.Generic;
using Farou.Utility;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUnitSpawner : Singleton<PlayerUnitSpawner>
{
    [SerializeField] private Transform baseTransform;
    [SerializeField] private Transform unitSpawnPoint;
    [SerializeField] private Button[] unitButtons; // Array to simplify button handling
    [SerializeField] private UnitHero[] unitHeroes; // Corresponding unit types for the buttons
    [SerializeField] private List<Unit> spawnedUnits = new List<Unit>();

    private new void Awake()
    {
        base.Awake();
        for (int i = 0; i < unitButtons.Length; i++)
        {
            int index = i; // Capture loop variable
            unitButtons[i].onClick.AddListener(() => OnUnitSpawn(unitHeroes[index]));
        }
    }

    private void OnEnable()
    {
        Unit.OnAnyUnitDead += PlayerUnit_OnAnyPlayerUnitDead;
        Projectile.OnProjectileAreaHit += EnemyUnit_OnProjectileAreaHit;
    }

    private void OnDisable()
    {
        Unit.OnAnyUnitDead -= PlayerUnit_OnAnyPlayerUnitDead;
        Projectile.OnProjectileAreaHit -= EnemyUnit_OnProjectileAreaHit;
    }

    private void PlayerUnit_OnAnyPlayerUnitDead(Unit unit)
    {
        if (unit && unit.UnitType == UnitType.Player)
        {
            PlayVFX(VisualEffectType.Dead, unit.transform.position);
            spawnedUnits.Remove(unit);
            // Destroy(unit.gameObject); // Consider pooling instead of destroying
            UnitObjectPool.Instance.ReturnToPool(unit.Stat.UnitHero, unit);
        }
    }

    private void EnemyUnit_OnProjectileAreaHit(Projectile projectile)
    {
        SpawnAreaHitEffect(projectile.transform.position);
    }

    public void SpawnAreaHitEffect(Vector3 position)
    {
        PlayVFX(VisualEffectType.Hit, position);
    }

    private void PlayVFX(VisualEffectType vfxType, Vector3 position)
    {
        var vfx = VisualEffectObjectPool.Instance.GetPooledObject(vfxType);
        if (vfx)
        {
            vfx.transform.position = position;
            vfx.Play();
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
