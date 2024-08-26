using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private const string ATTACK_PARAMETER = "Attack";

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void PlayAttackAnimation()
    {
        animator.SetBool(ATTACK_PARAMETER, true);
    }

    public void PlayIdleAnimation()
    {
        animator.SetBool(ATTACK_PARAMETER, false);
    }
}
