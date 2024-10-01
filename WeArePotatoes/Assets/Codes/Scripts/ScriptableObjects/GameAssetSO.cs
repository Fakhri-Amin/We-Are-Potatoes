using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameAssetSO", menuName = "Farou/Game Asset")]
public class GameAssetSO : ScriptableObject
{
    [System.Serializable]
    public class WorldSpriteReference
    {
        public MapType MapType;
        public List<Sprite> LevelMapSprites = new();
    }

    public Color BeachWorldColor;
    public Color ForestWorldColor;
    public List<WorldSpriteReference> WorldSpriteReferences = new();
}
