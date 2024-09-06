using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[System.Serializable]
public class UnitStatData
{
    [TabGroup("General")]
    [EnumPaging]
    public UnitHero UnitHero;
    [TabGroup("General")]
    [EnumPaging]
    public UnitRangeType UnitRangeType;
    [TabGroup("General")]
    [EnumPaging]
    public UnitAttackType UnitAttackType;

    [TabGroup("Stat")]
    public int Health;
    [TabGroup("Stat")]
    public int DamageAmount;
    [TabGroup("Stat")]
    public float MoveSpeed;
    [TabGroup("Stat")]
    public float DetectRadius;
    [TabGroup("Stat")]
    public float AttackRadius;
    [TabGroup("Stat")]
    public float AttackSpeed;
    [TabGroup("Stat")]
    [ShowIf("UnitAttackType", UnitAttackType.Area)] public float AreaOfEffectRadius;
}
