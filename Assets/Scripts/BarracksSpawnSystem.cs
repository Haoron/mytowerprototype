// BarracksSpawnSystem.cs
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;

[BurstCompile]
public partial struct BarracksSpawnSystem : ISystem
{
    public void OnUpdate(ref SystemState state)
    {
        var ecb = new EntityCommandBuffer(Allocator.Temp);

        foreach (var (building, transform) in SystemAPI
                     .Query<RefRW<Building>, RefRO<LocalTransform>>()
                     .WithAll<BarracksTag>())
        {
            building.ValueRW.SpawnTimer -= SystemAPI.Time.DeltaTime;
            if (building.ValueRW.SpawnTimer <= 0f)
            {
                building.ValueRW.SpawnTimer = building.ValueRO.SpawnRate;

                var spawnPos = transform.ValueRO.Position + building.ValueRO.SpawnOffset;
                var unit = ecb.Instantiate(building.ValueRO.UnitPrefab);
                ecb.SetComponent(unit, new LocalTransform
                {
                    Position = spawnPos,
                    Rotation = quaternion.identity,
                    Scale = 1
                });

                ecb.AddComponent(unit, new PlayerTag());
            }
        }

        ecb.Playback(state.EntityManager);
    }
}
