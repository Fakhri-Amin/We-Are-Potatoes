using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUnit : Unit
{
    [SerializeField] private SpriteRenderer spriteRenderer;

    public void SetBlink(float value)
    {
        Material mat = spriteRenderer.material;
        mat.SetFloat("_HitEffectBlend", value);
    }
}
