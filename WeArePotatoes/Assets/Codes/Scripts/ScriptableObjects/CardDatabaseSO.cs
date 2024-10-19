using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CardDatabaseSO", menuName = "Farou/Card Database")]
public class CardDatabaseSO : ScriptableObject
{
    public List<CardTypeConfig> CardTypeConfigs = new List<CardTypeConfig>();
    public List<CardLevelConfig> CardLevelConfigList = new List<CardLevelConfig>();
    public List<CardData> CardDatas = new List<CardData>();

    private void OnValidate()
    {
#if UNITY_EDITOR

        foreach (var item in CardDatas)
        {
            item.EffectDescription = $"+{item.EffectAmount}% {CardTypeConfigs.Find(i => i.CardType == item.CardType).EffectDescription}";
        }

#endif
    }
}

[System.Serializable]
public class CardLevelConfig
{
    public int Level;
    public int MaxCardAmount;
}

[System.Serializable]
public class CardTypeConfig
{
    public CardType CardType;
    public string EffectDescription;
}


