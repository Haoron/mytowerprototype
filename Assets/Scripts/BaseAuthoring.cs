// BaseAuthoring.cs (MonoBehaviour on the base object)
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class BaseAuthoring : MonoBehaviour
{
    public class Baker : Baker<BaseAuthoring>
    {
        public override void Bake(BaseAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new BaseTag());
            AddComponent(entity, new LocalTransform
            {
                Position = float3.zero,
                Rotation = quaternion.identity,
                Scale = 1
            });

            var slots = AddBuffer<ReservedBuildSlot>(entity);
            slots.Add(new ReservedBuildSlot { Offset = new float3(5, 0, 0), Occupied = false });
            slots.Add(new ReservedBuildSlot { Offset = new float3(-5, 0, 0), Occupied = false });
            slots.Add(new ReservedBuildSlot { Offset = new float3(0, 0, 5), Occupied = false });
            slots.Add(new ReservedBuildSlot { Offset = new float3(0, 0, -5), Occupied = false });
        }
    }
}

public struct BaseTag : IComponentData { }
