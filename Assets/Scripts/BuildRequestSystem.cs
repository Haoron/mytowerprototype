// BuildRequestSystem.cs
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

[BurstCompile]
public partial struct BuildRequestSystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<Gold>();
    }

    public void OnUpdate(ref SystemState state)
    {
        var ecb = new EntityCommandBuffer(Allocator.Temp);

        foreach (var (req, entity) in SystemAPI.Query<RefRO<BuildRequest>>().WithEntityAccess())
        {
            var gold = SystemAPI.GetSingletonRW<Gold>();
            if (gold.ValueRO.Amount >= req.ValueRO.Cost)
            {
                gold.ValueRW.Amount -= req.ValueRO.Cost;

                // Find nearest free spot. For demo, use a simple ring increment.
                float radius = 5f;
                var pos = req.ValueRO.BasePosition + new float3(radius, 0, 0); // TODO: add more sophisticated placement

                var buildingEntity = ecb.Instantiate(req.ValueRO.Prefab);
                ecb.SetComponent(buildingEntity, new LocalTransform
                {
                    Position = pos,
                    Rotation = quaternion.identity,
                    Scale = 1
                });

                // Add the BarracksTag or other data. If prefab already has Building, it will be carried over.
            }

            ecb.DestroyEntity(entity);
        }

        ecb.Playback(state.EntityManager);
    }
}
