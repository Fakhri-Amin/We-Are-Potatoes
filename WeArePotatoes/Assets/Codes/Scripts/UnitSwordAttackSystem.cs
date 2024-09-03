using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSwordAttackSystem : MonoBehaviour
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
            unit.TargetUnit.Damage(unit.Stat.DamageAmount);
        }
    }
}
