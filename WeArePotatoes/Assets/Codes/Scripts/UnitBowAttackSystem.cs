using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;

public class UnitBowAttackSystem : MonoBehaviour
{
    [SerializeField] private Projectile projectile;
    [SerializeField] private Transform shootingTransform;
    private ObjectPool<Projectile> objectPool;
    private Unit unit;

    private void Awake()
    {
        unit = GetComponent<Unit>();
    }

    private void Start()
    {
        objectPool = new ObjectPool<Projectile>(() =>
        {
            return Instantiate(projectile);
        }, obj =>
        {
            obj.gameObject.SetActive(true);
            obj.transform.position = shootingTransform.position;
        }, obj =>
        {
            obj.gameObject.SetActive(false);
        }, obj =>
        {
            Destroy(obj.gameObject);
        }, false, 5, 10);
    }

    public Projectile GetPooledObject()
    {
        return objectPool.Get();
    }

    public void ReturnToPool(Projectile projectile)
    {
        objectPool.Release(projectile);
    }

    public virtual void HandleAttack()
    {
        if (unit.AttackableTarget != null)
        {
            var newProjectile = GetPooledObject();

            Unit target = unit.AttackableTarget as Unit;

            newProjectile.Initialize(this, unit.UnitType, target.transform, unit.DamageAmount, 15);
        }
    }
}
