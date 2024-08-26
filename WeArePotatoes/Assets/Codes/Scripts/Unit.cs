using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Unit : MonoBehaviour, IAttackable
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private Vector3 moveDirection;

    protected bool canMove = true;

    protected void Update()
    {
        if (canMove)
        {
            Move(moveDirection, moveSpeed);
        }
    }

    private void Move(Vector3 moveDirection, float moveSpeed)
    {
        transform.position += moveSpeed * Time.deltaTime * moveDirection;
    }

    public abstract void Attack();

    public void Damage()
    {

    }
}
