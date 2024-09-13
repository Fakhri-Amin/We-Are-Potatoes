using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BaseBuildingSO", menuName = "Farou/Base Building")]
public class BaseBuildingSO : ScriptableObject
{
    public float SeedProductionRate;
    public int BaseUpgradeSeedCostPrice;
    public int BaseHealth;
}
