using Unity.Entities;
using Unity.Mathematics;

public struct BuildRequest : IComponentData
{
    public Entity Prefab;
    public int Cost;
    public float3 BasePosition;
}