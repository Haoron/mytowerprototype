// PlayerUnitAuthoring.cs
using Unity.Entities;
using UnityEngine;

public class PlayerUnitAuthoring : MonoBehaviour
{
    public float MoveSpeed = 5f;
    public int Damage = 1;
    public float AttackRange = 1.5f;
    public float AttackCooldown = 1f;
    public int HealthValue = 10;

    public class Baker : Baker<PlayerUnitAuthoring>
    {
        public override void Bake(PlayerUnitAuthoring authoring)
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
            AddComponent<PlayerTag>(entity);
        }
    }
}
