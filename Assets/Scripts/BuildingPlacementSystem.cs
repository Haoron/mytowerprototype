using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

public struct ReservedBuildSlot : IBufferElementData
{
    public float3 Offset;
    public bool Occupied;
}

public struct BuildPosition : IComponentData
{
    public float3 Value;
}

[BurstCompile]
public partial struct BuildingPlacementSystem : ISystem
{
    public void OnUpdate(ref SystemState state)
    {
        if (!SystemAPI.HasSingleton<BaseTag>())
            return;

        var baseEntity = SystemAPI.GetSingletonEntity<BaseTag>();
        var baseTransform = SystemAPI.GetComponent<LocalTransform>(baseEntity);
        var slots = SystemAPI.GetBuffer<ReservedBuildSlot>(baseEntity);
        var ecb = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);

        foreach (var (req, entity) in SystemAPI.Query<RefRO<BuildRequest>>().WithNone<BuildPosition>().WithEntityAccess())
        {
            for (int i = 0; i < slots.Length; i++)
            {
                var slot = slots[i];
                if (slot.Occupied)
                    continue;

                slot.Occupied = true;
                slots[i] = slot;
                var pos = baseTransform.Position + slot.Offset;
                ecb.AddComponent(entity, new BuildPosition { Value = pos });
                break;
            }
        }
    }
}
