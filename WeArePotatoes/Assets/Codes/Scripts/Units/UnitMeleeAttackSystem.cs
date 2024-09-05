using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitMeleeAttackSystem : MonoBehaviour
{
    private Unit unit;

    private void Awake()
    {
        unit = GetComponent<Unit>();
    }

    public virtual void HandleAttack()
    {
        if (unit.TargetUnit != null)
        {
            if (unit.Stat.UnitAttackType == UnitAttackType.Single)
            {
                unit.TargetUnit.Damage(unit.Stat.DamageAmount);
            }
            else if (unit.Stat.UnitAttackType == UnitAttackType.Area)
            {
                Collider2D[] targetsInRadius = Physics2D.OverlapCircleAll(transform.position, unit.Stat.AreaOfEffectRadius, unit.TargetMask);

                foreach (var item in targetsInRadius)
                {
                    if (item.TryGetComponent<Unit>(out Unit unit) && unit.UnitType != unit.UnitType)
                    {
                        var vfx = VisualEffectObjectPool.Instance.GetPooledObject(VisualEffectType.Hit);
                        vfx.Play();
                        unit.Damage(unit.Stat.DamageAmount);
                    }
                }
            }
        }
    }
}
