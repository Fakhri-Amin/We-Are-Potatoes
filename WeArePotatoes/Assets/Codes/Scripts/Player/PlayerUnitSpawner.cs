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
    [SerializeField] private ParticleSystem unitDeadVFX;
    [SerializeField] private ParticleSystem projectileHitVFX;
    [SerializeField] private List<Unit> spawnedUnits = new List<Unit>();

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
        Projectile.OnProjectileAreaHit += EnemyUnit_OnProjectileAreaHit;
    }

    private void PlayerUnit_OnAnyPlayerUnitDead(Unit unit)
    {
        if (unit && unit.UnitType == UnitType.Player)
        {
            // var vfx = Instantiate(unitDeadVFX, unit.transform.position, Quaternion.identity);
            // vfx.gameObject.SetActive(true);

            var vfx = VisualEffectObjectPool.Instance.GetPooledObject(VisualEffectType.Dead);
            vfx.transform.position = unit.transform.position;
            vfx.Play();

            spawnedUnits.Remove(unit);
            Destroy(unit.gameObject);
        }
    }

    private void EnemyUnit_OnProjectileAreaHit(Projectile projectile)
    {
        SpawnAreaHitEffect(projectile.transform.position);
    }

    public void SpawnAreaHitEffect(Vector3 position)
    {
        // var vfx = Instantiate(projectileHitVFX, position, Quaternion.identity);
        // vfx.gameObject.SetActive(true);

        var vfx = VisualEffectObjectPool.Instance.GetPooledObject(VisualEffectType.Hit);
        vfx.transform.position = position;
        vfx.Play();
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

    public Vector3 GetUnitPosition(Unit unit) => spawnedUnits.Find(i => i == unit).gameObject.transform.position;

    private void OnUnitSpawn(Unit unit)
    {
        Vector3 offset = new Vector3(0, Random.Range(-0.5f, 0.5f), 0);
        Unit spawnedUnit = Instantiate(unit, playerUnitSpawnPoint.position + offset, Quaternion.identity);
        spawnedUnit.InitializeUnit(UnitType.Player, baseTransform.position);
        spawnedUnits.Add(spawnedUnit);
    }
}
