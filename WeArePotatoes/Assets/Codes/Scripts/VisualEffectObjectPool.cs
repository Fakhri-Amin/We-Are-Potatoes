using System.Collections;
using System.Collections.Generic;
using Farou.Utility;
using UnityEngine;
using UnityEngine.Pool;

public class VisualEffectObjectPool : Singleton<VisualEffectObjectPool>
{
    [System.Serializable]
    public class VisualEffectData
    {
        public VisualEffectType Type;
        public ParticleSystem Effect;
        public Transform ParentTransform;
        public ObjectPool<ParticleSystem> ObjectPool;
    }

    [SerializeField] private List<VisualEffectData> visualEffectDatas = new List<VisualEffectData>();
    private void Start()
    {
        foreach (var item in visualEffectDatas)
        {
            item.ObjectPool = new ObjectPool<ParticleSystem>(() =>
            {
                return Instantiate(item.Effect, item.ParentTransform);
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

    public ParticleSystem GetPooledObject(VisualEffectType type)
    {
        return visualEffectDatas.Find(i => i.Type == type).ObjectPool.Get();
    }

    public void ReturnToPool(VisualEffectType type, ParticleSystem effect)
    {
        visualEffectDatas.Find(i => i.Type == type).ObjectPool.Release(effect);
    }
}

public enum VisualEffectType
{
    Hit,
    Dead
}
