// UnitAISystem.cs
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

[BurstCompile]
public partial struct UnitAISystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<BaseTag>();
    }

    public void OnUpdate(ref SystemState state)
    {
        var baseTransform = SystemAPI.GetComponentRO<LocalTransform>(
            SystemAPI.GetSingletonEntity<BaseTag>()).ValueRO;
        float delta = SystemAPI.Time.DeltaTime;

        var ecb = new EntityCommandBuffer(Allocator.Temp);

        // Move enemies toward base
        foreach (var (transform, unit) in SystemAPI.Query<RefRW<LocalTransform>, RefRW<Unit>>().WithAll<EnemyTag>())
        {
            float3 dir = math.normalize(baseTransform.Position - transform.ValueRO.Position);
            transform.ValueRW.Position += dir * unit.ValueRO.MoveSpeed * delta;
        }

        // Very simple targeting: each player unit moves toward the closest enemy.
        foreach (var (transform, unit, entity) in SystemAPI.Query<RefRW<LocalTransform>, RefRW<Unit>>()
                     .WithAll<PlayerTag>().WithEntityAccess())
        {
            // Find closest enemy (naïve approach)
            float closestDist = float.MaxValue;
            float3 targetPos = float3.zero;
            Entity targetEnemy = Entity.Null;

            foreach (var (enemyTransform, enemyEntity) in SystemAPI.Query<RefRO<LocalTransform>>()
                         .WithAll<EnemyTag>().WithEntityAccess())
            {
                float dist = math.distance(transform.ValueRO.Position, enemyTransform.ValueRO.Position);
                if (dist < closestDist)
                {
                    closestDist = dist;
                    targetPos = enemyTransform.ValueRO.Position;
                    targetEnemy = enemyEntity;
                }
            }

            if (targetEnemy == Entity.Null) continue;

            // Move toward or attack
            if (closestDist > unit.ValueRO.AttackRange)
            {
                var dir = math.normalize(targetPos - transform.ValueRO.Position);
                transform.ValueRW.Position += dir * unit.ValueRO.MoveSpeed * delta;
            }
            else
            {
                unit.ValueRW.CooldownTimer -= delta;
                if (unit.ValueRW.CooldownTimer <= 0f)
                {
                    unit.ValueRW.CooldownTimer = unit.ValueRO.AttackCooldown;

                    if (SystemAPI.HasComponent<Health>(targetEnemy))
                    {
                        var health = SystemAPI.GetComponentRW<Health>(targetEnemy);
                        health.ValueRW.Value -= unit.ValueRO.Damage;
                        if (health.ValueRO.Value <= 0)
                            ecb.DestroyEntity(targetEnemy);
                    }
                }
            }
        }

        // Enemy attacking the base or player units would follow a similar pattern.
        ecb.Playback(state.EntityManager);
    }
}
