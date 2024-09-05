using System.Collections;
using System.Collections.Generic;
using Farou.Utility;
using UnityEngine;
using UnityEngine.Pool;

public class UnitObjectPool : Singleton<UnitObjectPool>
{
    [System.Serializable]
    public class UnitHeroReference
    {
        public UnitHero Type;
        public Unit Unit;
        [HideInInspector] public Transform ParentTransform;
        [HideInInspector] public ObjectPool<Unit> ObjectPool;
    }

    [SerializeField] private List<UnitHeroReference> unitHeroReferences = new List<UnitHeroReference>();

    private void Start()
    {
        foreach (var item in unitHeroReferences)
        {
            item.ObjectPool = new ObjectPool<Unit>(() =>
            {
                if (item.ParentTransform == null)
                {
                    item.ParentTransform = Instantiate(new GameObject(), transform).transform;
                    item.ParentTransform.name = item.Type.ToString();
                }
                return Instantiate(item.Unit, item.ParentTransform);
            }, obj =>
            {
                obj.gameObject.SetActive(true);
            }, obj =>
            {
                obj.gameObject.SetActive(false);
            }, obj =>
            {
                Destroy(obj.gameObject);
            }, false, 10, 20);
        }
    }

    public Unit GetPooledObject(UnitHero type)
    {
        return unitHeroReferences.Find(i => i.Type == type).ObjectPool.Get();
    }

    public void ReturnToPool(UnitHero type, Unit effect)
    {
        unitHeroReferences.Find(i => i.Type == type).ObjectPool.Release(effect);
    }
}


