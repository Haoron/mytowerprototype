// BuildUI.cs
using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;

public class BuildUI : MonoBehaviour
{
    public int BarracksCost = 100;
    public Entity BarracksPrefab;
    public BaseAuthoring BaseRef;
    public UnityEngine.InputSystem.InputAction BuildBarracksAction;

    void OnEnable() => BuildBarracksAction.Enable();
    void OnDisable() => BuildBarracksAction.Disable();

    void Update()
    {
        if (BuildBarracksAction.WasPressedThisFrame())
            TryBuild(BarracksPrefab, BarracksCost);
    }

    void TryBuild(Entity prefab, int cost)
    {
        var ecb = World.DefaultGameObjectInjectionWorld
                     .GetOrCreateSystemManaged<BeginSimulationEntityCommandBufferSystem>()
                     .CreateCommandBuffer();

        var request = ecb.CreateEntity();
        ecb.AddComponent(request, new BuildRequest
        {
            Prefab = prefab,
            Cost = cost,
            BasePosition = BaseRef.transform.position
        });
    }
}