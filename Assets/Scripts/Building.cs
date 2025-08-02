// Building.cs
using Unity.Entities;
using Unity.Mathematics;

public struct Building : IComponentData
{
    public float3 SpawnOffset;     // Where units spawn around the building
    public float SpawnRate;        // Seconds per unit
    public float SpawnTimer;       // Internal timer
    public Entity UnitPrefab;
    public int Cost;               // Gold cost
}


