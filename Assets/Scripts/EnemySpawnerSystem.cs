// EnemySpawnerSystem.cs
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Collections;

[BurstCompile]
public partial struct EnemySpawnerSystem : ISystem
{
    public float SpawnRadius;
    public float SpawnInterval;
    float _timer;

    public void OnCreate(ref SystemState state)
    {
        SpawnRadius = 20f;
        SpawnInterval = 3f;
        _timer = SpawnInterval;
        state.RequireForUpdate<BaseTag>();  // Need base reference
        // Do not run until an enemy prefab exists to avoid GetSingleton errors
        state.RequireForUpdate<EnemyPrefab>();
    }

    public void OnUpdate(ref SystemState state)
    {
        _timer -= SystemAPI.Time.DeltaTime;
        if (_timer > 0f) return;
        _timer = SpawnInterval;

        var ecb = new EntityCommandBuffer(Allocator.Temp);

        var random = Unity.Mathematics.Random.CreateFromIndex((uint)System.DateTime.Now.Ticks);
        float angle = random.NextFloat(0, math.PI * 2);
        float3 spawnPos = new float3(math.cos(angle), 0, math.sin(angle)) * SpawnRadius;

        // Create the enemy entity (assume an Enemy prefab exists)
        var enemyPrefab = SystemAPI.GetSingleton<EnemyPrefab>().Value;
        var enemy = ecb.Instantiate(enemyPrefab);
        ecb.SetComponent(enemy, new LocalTransform { Position = spawnPos, Rotation = quaternion.identity, Scale = 1 });
        ecb.AddComponent(enemy, new EnemyTag());

        ecb.Playback(state.EntityManager);
    }
}
