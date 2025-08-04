using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

[DisallowMultipleComponent]
public class BuildingPrefabsAuthoring : MonoBehaviour
{
    [System.Serializable]
    public struct Entry
    {
        public GameObject Prefab;
        public int Cost;
    }

    public List<Entry> Prefabs = new();

    public class Baker : Baker<BuildingPrefabsAuthoring>
    {
        public override void Bake(BuildingPrefabsAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.None);
            AddComponent<BuildingPrefabRegistry>(entity);
            var buffer = AddBuffer<BuildingPrefabElement>(entity);
            foreach (var entry in authoring.Prefabs)
            {
                if (entry.Prefab == null)
                    continue;
                var prefabEntity = GetEntity(entry.Prefab, TransformUsageFlags.Dynamic);
                buffer.Add(new BuildingPrefabElement { Prefab = prefabEntity, Cost = entry.Cost });
            }
        }
    }
}

public struct BuildingPrefabRegistry : IComponentData { }

public struct BuildingPrefabElement : IBufferElementData
{
    public Entity Prefab;
    public int Cost;
}
