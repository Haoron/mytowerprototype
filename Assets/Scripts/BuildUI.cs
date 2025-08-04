// BuildUI.cs
using UnityEngine;
using Unity.Entities;
using UnityEngine.UI;

public class BuildUI : MonoBehaviour
{
    public int BarracksIndex = 0;
    public UnityEngine.InputSystem.InputAction BuildBarracksAction;
    void Start()
    {
        // Hook up UI button automatically so no cross-scene reference is needed.
        var buttonObj = GameObject.Find("BarracksButton");
        if (buttonObj != null)
        {
            var btn = buttonObj.GetComponent<Button>();
            if (btn != null)
                btn.onClick.AddListener(BuildBarracks);
        }
    }

    void OnEnable() => BuildBarracksAction.Enable();
    void OnDisable() => BuildBarracksAction.Disable();

    void Update()
    {
        if (BuildBarracksAction.WasPressedThisFrame())
            TryBuild(BarracksIndex);
    }

    // Allow UI buttons to trigger barracks construction.
    public void BuildBarracks()
    {
        TryBuild(BarracksIndex);
    }

    void TryBuild(int prefabIndex)
    {
        var world = World.DefaultGameObjectInjectionWorld;
        if (world == null)
            return;
        var em = world.EntityManager;
        var query = em.CreateEntityQuery(ComponentType.ReadOnly<BuildingPrefabRegistry>());
        if (query.CalculateEntityCount() == 0)
            return;

        var registryEntity = query.GetSingletonEntity();
        var buffer = em.GetBuffer<BuildingPrefabElement>(registryEntity);
        if (prefabIndex < 0 || prefabIndex >= buffer.Length)
            return;

        var element = buffer[prefabIndex];

        var ecb = world
                     .GetOrCreateSystemManaged<BeginSimulationEntityCommandBufferSystem>()
                     .CreateCommandBuffer();

        var request = ecb.CreateEntity();
        ecb.AddComponent(request, new BuildRequest
        {
            Prefab = element.Prefab,
            Cost = element.Cost
        });
    }
}
