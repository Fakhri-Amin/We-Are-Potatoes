using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[System.Serializable]
public class UnitStatData
{
    public UnitHero UnitHero;
    public UnitRangeType UnitRangeType;
    public UnitAttackType UnitAttackType;
    public int Health;
    public int DamageAmount;
    public float MoveSpeed;
    public float DetectRadius;
    public float AttackRadius;
    public float AttackSpeed;
    [ShowIf("UnitAttackType", UnitAttackType.Area)] public float AreaOfEffectRadius;
}
