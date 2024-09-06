using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "UnitStatSO", menuName = "Farou/Unit Stat")]
public class UnitStatSO : ScriptableObject
{
    [TableList(ShowIndexLabels = true)] public List<UnitStatData> UnitStatDataList;
}
