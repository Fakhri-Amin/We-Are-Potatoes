using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelWaveSO", menuName = "Farou/Level Wave")]
public class LevelWaveSO : ScriptableObject
{
    public int LevelIndex;
    public UnitHero UnitReward;
    public float DelayAtStart;
    public float DelayBetweenWaves;
    public int BaseHealth;
    [TableList(ShowIndexLabels = true)] public List<WaveData> WaveDatas = new();
}
