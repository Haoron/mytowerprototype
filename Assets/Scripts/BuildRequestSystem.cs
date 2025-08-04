// BuildRequestSystem.cs
using Unity.Burst;
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
        var ecb = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);

        foreach (var (req, pos, entity) in SystemAPI.Query<RefRO<BuildRequest>, RefRO<BuildPosition>>().WithEntityAccess())
        {
            var gold = SystemAPI.GetSingletonRW<Gold>();
            if (gold.ValueRO.Amount >= req.ValueRO.Cost)
            {
                gold.ValueRW.Amount -= req.ValueRO.Cost;
                var buildingEntity = ecb.Instantiate(req.ValueRO.Prefab);
                ecb.SetComponent(buildingEntity, new LocalTransform
                {
                    Position = pos.ValueRO.Value,
                    Rotation = quaternion.identity,
                    Scale = 1
                });
            }

            ecb.DestroyEntity(entity);
        }
    }
}
