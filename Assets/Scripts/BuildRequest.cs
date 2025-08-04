using Unity.Entities;
public struct BuildRequest : IComponentData
{
    public Entity Prefab;
    public int Cost;
}