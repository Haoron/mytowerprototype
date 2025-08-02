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
        }
    }
}

public struct BaseTag : IComponentData { }
