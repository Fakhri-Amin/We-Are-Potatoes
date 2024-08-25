using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Unit : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private Vector3 moveDirection;

    private void Update()
    {
        transform.position += moveSpeed * Time.deltaTime * moveDirection;
    }

    public abstract void Attack();
}
