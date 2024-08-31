using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private int damageAmount;
    private Rigidbody2D rb;
    private float moveSpeed;
    private Transform targetTransform;
    private UnitType unitType;
    private UnitBowAttackSystem unitBowAttackSystem;

    public void Initialize(UnitBowAttackSystem unitBowAttackSystem, UnitType unitType, Transform targetTransform, int damageAmount, float moveSpeed)
    {
        this.unitBowAttackSystem = unitBowAttackSystem;
        this.unitType = unitType;
        this.targetTransform = targetTransform;
        this.damageAmount = damageAmount;
        this.moveSpeed = moveSpeed;

        // Set the direction
        Vector3 direction = targetTransform.position - transform.position;
        rb.velocity = new Vector2(direction.x, direction.y).normalized * moveSpeed;

        // Set the rotation
        float rotation = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, rotation);
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<IAttackable>(out IAttackable attackable))
        {
            Unit target = attackable as Unit;
            if (target.UnitType != unitType)
            {
                attackable.Damage(damageAmount);
                unitBowAttackSystem.ReturnToPool(this);
            }
        }
    }
}
