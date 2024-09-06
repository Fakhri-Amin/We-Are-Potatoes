using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class UnitAttackSystem : MonoBehaviour
{
    [SerializeField] private UnitRangeType unitRangeType;
    [SerializeField] protected Transform shootingTransform;
    [HideIf("unitRangeType", UnitRangeType.Melee)]
    [SerializeField] protected ProjectileType projectileType;

    private Unit unit;

    private void Awake()
    {
        unit = GetComponent<Unit>();
    }

    public virtual void HandleAttack()
    {
        if (unit.TargetUnit == null) return; // No target, no attack

        if (unitRangeType == UnitRangeType.Melee)
        {
            HandleMeleeAttack();
        }
        else
        {
            HandleRangedAttack();
        }
    }

    private void HandleMeleeAttack()
    {
        if (unit.Stat.UnitAttackType == UnitAttackType.Single)
        {
            PerformSingleTargetMeleeAttack();
        }
        else if (unit.Stat.UnitAttackType == UnitAttackType.Area)
        {
            PerformAreaOfEffectAttack();
        }
    }

    private void PerformSingleTargetMeleeAttack()
    {
        unit.TargetUnit.Damage(unit.Stat.DamageAmount);
    }

    private void PerformAreaOfEffectAttack()
    {
        Collider2D[] targetsInRadius = Physics2D.OverlapCircleAll(
            shootingTransform.position,
            unit.Stat.AreaOfEffectRadius,
            unit.TargetMask
        );

        foreach (var target in targetsInRadius)
        {
            if (target.TryGetComponent(out Unit targetUnit) && targetUnit.UnitType != unit.UnitType)
            {
                targetUnit.Damage(unit.Stat.DamageAmount);
            }
        }
    }

    private void HandleRangedAttack()
    {
        var newProjectile = ProjectileObjectPool.Instance.GetPooledObject(projectileType);

        if (newProjectile != null)
        {
            newProjectile.transform.position = shootingTransform.position;
            newProjectile.Initialize(unit, unit.TargetUnit, projectileType);
        }
    }
}
