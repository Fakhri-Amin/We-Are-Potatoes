using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DungeonLevelSO", menuName = "Farou/Dungeon Level")]
public class DungeonLevelSO : ScriptableObject
{
    [System.Serializable]
    public class DungeonLevelReference
    {
        public LevelWaveSO LevelWaveSO;
        public int EntryLimit;
    }

    public List<DungeonLevelReference> DungeonLevelReferences = new List<DungeonLevelReference>();
}
