using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "UnitStatSO", menuName = "Farou/Unit Stat")]
public class UnitDataSO : ScriptableObject
{
    public int PlayerBaseMaxHealth;
    public int EnemyBaseMaxHealth;
    [TableList(ShowIndexLabels = true)] public List<UnitData> UnitStatDataList;
}
