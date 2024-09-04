using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitCatapultAttackSystem : UnitRangeAttackSystem
{
    public void HandleAttack()
    {
        if (unit.TargetUnit != null)
        {
            var newProjectile = ProjectileObjectPool.Instance.GetPooledObject(projectileType);

            // newProjectile.transform.parent = null;
            newProjectile.transform.position = shootingTransform.position;

            // Unit target = unit.AttackableTarget as Unit;

            newProjectile.Initialize(unit, unit.TargetUnit, projectileType);
        }
    }
}
