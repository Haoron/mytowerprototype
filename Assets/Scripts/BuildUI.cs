// BuildUI.cs
using UnityEngine;
using Unity.Entities;

public class BuildUI : MonoBehaviour
{
    public int BarracksCost = 100;
    public Entity BarracksPrefab;
    public BaseAuthoring BaseRef;
    public UnityEngine.InputSystem.InputAction BuildBarracksAction;

    Entity _barracksPrefabEntity;

    void Start()
    {
        _barracksPrefabEntity = BarracksPrefab;
    }

    void OnEnable() => BuildBarracksAction.Enable();
    void OnDisable() => BuildBarracksAction.Disable();

    void Update()
    {
        if (BuildBarracksAction.WasPressedThisFrame())
            TryBuild(_barracksPrefabEntity, BarracksCost);
    }

    // Allow UI buttons to trigger barracks construction.
    public void BuildBarracks()
    {
        TryBuild(_barracksPrefabEntity, BarracksCost);
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
