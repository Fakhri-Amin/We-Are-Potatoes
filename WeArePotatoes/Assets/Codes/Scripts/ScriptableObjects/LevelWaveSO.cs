using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelWaveSO", menuName = "Farou/Level Wave")]
public class LevelWaveSO : ScriptableObject
{
    public float DelayAtStart;
    public float DelayBetweenWaves;
    [TableList(ShowIndexLabels = true)] public List<WaveData> WaveDatas = new();
}
