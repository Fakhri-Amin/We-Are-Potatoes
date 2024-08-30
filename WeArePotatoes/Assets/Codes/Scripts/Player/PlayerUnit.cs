using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerUnit : Unit
{
    [SerializeField] private SpriteRenderer spriteRenderer;

    public void SetBlink(float value)
    {
        Material mat = spriteRenderer.material;
        mat.SetFloat("_HitEffectBlend", value);
    }
}
