using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[System.Serializable]
public class UnitData
{
    [Title("@UnitHero", titleAlignment: TitleAlignments.Centered)]
    [TabGroup("General")]
    [EnumPaging]
    public UnitHero UnitHero;
    [TabGroup("General")]
    [EnumPaging]
    public UnitRangeType UnitRangeType;
    [TabGroup("General")]
    [EnumPaging]
    public UnitAttackType UnitAttackType;
    [TabGroup("General")]
    public Unit Prefab;
    [TabGroup("General")]
    public Sprite Sprite;
    [TabGroup("General")]
    public string Name;

    [Title("@UnitHero", titleAlignment: TitleAlignments.Centered)]
    [TabGroup("Stat")]
    public int SeedCost;
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
    [HideIf("UnitRangeType", UnitRangeType.Melee)] public float ProjectileSpeed;
    [TabGroup("Stat")]
    [ShowIf("UnitAttackType", UnitAttackType.Area)] public float AreaOfEffectRadius;
}
