using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UnitStatSO", menuName = "Farou/Unit Stat")]
public class UnitStatSO : ScriptableObject
{
    public List<UnitStatData> UnitStatDataList;
}
