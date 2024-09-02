using System.Collections;
using System.Collections.Generic;
using Farou.Utility;
using UnityEngine;
using UnityEngine.Pool;

public class ProjectileObjectPool : Singleton<ProjectileObjectPool>
{
    [System.Serializable]
    public class ProjectileData
    {
        public ProjectileType ProjectileType;
        public Projectile Projectile;
        public Transform ParentTransform;
        public ObjectPool<Projectile> ObjectPool;
    }

    [SerializeField] private List<ProjectileData> projectileDatas = new List<ProjectileData>();

    private void Start()
    {
        foreach (var item in projectileDatas)
        {
            item.ObjectPool = new ObjectPool<Projectile>(() =>
            {
                return Instantiate(item.Projectile, item.ParentTransform);
            }, obj =>
            {
                obj.gameObject.SetActive(true);
            }, obj =>
            {
                obj.gameObject.SetActive(false);
            }, obj =>
            {
                Destroy(obj.gameObject);
            }, false, 5, 10);
        }
    }

    public Projectile GetPooledObject(ProjectileType type)
    {
        return projectileDatas.Find(i => i.ProjectileType == type).ObjectPool.Get();
    }

    public void ReturnToPool(ProjectileType type, Projectile projectile)
    {
        projectileDatas.Find(i => i.ProjectileType == type).ObjectPool.Release(projectile);
    }
}
