using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitRangeAttackSystem : MonoBehaviour
{
    [SerializeField] protected Transform shootingTransform;
    [SerializeField] protected ProjectileType projectileType;
    protected Unit unit;

    private void Awake()
    {
        unit = GetComponent<Unit>();
    }

    public void HandleAttack()
    {
        if (unit.TargetUnit != null)
        {
            var newProjectile = ProjectileObjectPool.Instance.GetPooledObject(projectileType);

            newProjectile.transform.position = shootingTransform.position;

            // Unit target = unit.AttackableTarget as Unit;

            newProjectile.Initialize(unit, unit.TargetUnit, projectileType);
        }
    }
}
