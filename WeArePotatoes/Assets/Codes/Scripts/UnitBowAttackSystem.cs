using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;

public class UnitBowAttackSystem : UnitRangeAttackSystem
{
    public void HandleAttack()
    {
        if (unit.TargetUnit != null)
        {
            var newProjectile = ProjectileObjectPool.Instance.GetPooledObject(projectileType);

            newProjectile.transform.position = shootingTransform.position;

            // Unit target = unit.AttackableTarget as Unit;

            newProjectile.Initialize(unit.UnitType, unit.Stat, projectileType, unit.TargetUnit);
        }
    }
}
