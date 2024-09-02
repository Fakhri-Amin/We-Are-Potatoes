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
}
