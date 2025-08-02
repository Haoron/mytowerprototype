// BarracksAuthoring.cs
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class BarracksAuthoring : MonoBehaviour
{
    public Vector3 SpawnOffset;
    public float SpawnRate = 5f;
    public GameObject UnitPrefab;
    public int Cost = 100;

    public class Baker : Baker<BarracksAuthoring>
    {
        public override void Bake(BarracksAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new Building
            {
                SpawnOffset = authoring.SpawnOffset,
                SpawnRate = authoring.SpawnRate,
                SpawnTimer = authoring.SpawnRate,
                UnitPrefab = GetEntity(authoring.UnitPrefab, TransformUsageFlags.Dynamic),
                Cost = authoring.Cost
            });
            AddComponent<BarracksTag>(entity);
        }
    }
}
