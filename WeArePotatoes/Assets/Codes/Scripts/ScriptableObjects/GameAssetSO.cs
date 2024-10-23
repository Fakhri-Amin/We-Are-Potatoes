using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameAssetSO", menuName = "Farou/Game Asset")]
public class GameAssetSO : ScriptableObject
{
    public Sprite GoldCoinSprite;
    public Sprite AzureCoinSprite;
    public List<WorldSpriteReference> WorldSpriteReferences = new();
}
