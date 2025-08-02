// EnemyPrefabAuthoring.cs
using Unity.Entities;
using UnityEngine;

public class EnemyPrefabAuthoring : MonoBehaviour
{
    public GameObject EnemyPrefab;

    public class Baker : Baker<EnemyPrefabAuthoring>
    {
        public override void Bake(EnemyPrefabAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.None);
            AddComponent(entity, new EnemyPrefab
            {
                Value = GetEntity(authoring.EnemyPrefab, TransformUsageFlags.Dynamic)
            });
        }
    }
}
