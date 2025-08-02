using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public struct BuildRequest : IComponentData
{
    public GameObject Prefab;
    public int Cost;
    public float3 BasePosition;
}