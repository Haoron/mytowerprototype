// Unit.cs
using Unity.Entities;

public struct Unit : IComponentData
{
    public float MoveSpeed;
    public int Damage;
    public float AttackRange;
    public float AttackCooldown;
    public float CooldownTimer;
}
