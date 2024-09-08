using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttackable
{
    public GameObject GameObject { get; }
    public void Damage(int damageAmount);
}
