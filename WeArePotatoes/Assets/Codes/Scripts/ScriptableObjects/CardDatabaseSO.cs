using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CardDatabaseSO", menuName = "Farou/Card Database")]
public class CardDatabaseSO : ScriptableObject
{
    public List<CardLevelConfig> CardLevelConfigList = new List<CardLevelConfig>();
    public List<CardData> CardDatas = new List<CardData>();
}

[System.Serializable]
public class CardLevelConfig
{
    public int Level;
    public int MaxCardAmount;
}
