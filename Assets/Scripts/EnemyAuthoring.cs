// EnemyAuthoring.cs
using Unity.Entities;
using UnityEngine;

public class EnemyAuthoring : MonoBehaviour
{
    public float MoveSpeed = 3f;
    public int Damage = 1;
    public float AttackRange = 1f;
    public float AttackCooldown = 1f;
    public int HealthValue = 5;

    public class Baker : Baker<EnemyAuthoring>
    {
        public override void Bake(EnemyAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new Unit
            {
                MoveSpeed = authoring.MoveSpeed,
                Damage = authoring.Damage,
                AttackRange = authoring.AttackRange,
                AttackCooldown = authoring.AttackCooldown,
                CooldownTimer = 0f
            });
            AddComponent(entity, new Health { Value = authoring.HealthValue });
            AddComponent<EnemyTag>(entity);
        }
    }
}
