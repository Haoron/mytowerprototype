// BuildUI.cs
using UnityEngine;
using Unity.Entities;
using UnityEngine.UI;

public class BuildUI : MonoBehaviour
{
    public int BarracksCost = 100;
    public Entity BarracksPrefab;
    public BaseAuthoring BaseRef;
    public UnityEngine.InputSystem.InputAction BuildBarracksAction;

    Entity _barracksPrefabEntity;

    void Start()
    {
        // Cache entity prefab passed from inspector. If none provided, attempt to
        // locate one converted in the scene by searching for the prefab tag.
        _barracksPrefabEntity = BarracksPrefab;

        if (_barracksPrefabEntity == Entity.Null)
        {
            var em = World.DefaultGameObjectInjectionWorld.EntityManager;
            var query = em.CreateEntityQuery(typeof(BarracksTag), typeof(Building), typeof(Unity.Entities.Prefab));
            if (query.CalculateEntityCount() > 0)
                _barracksPrefabEntity = query.GetSingletonEntity();
        }

        // Locate base authoring if not linked in inspector.
        if (BaseRef == null)
            BaseRef = FindObjectOfType<BaseAuthoring>();

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
            TryBuild(_barracksPrefabEntity, BarracksCost);
    }

    // Allow UI buttons to trigger barracks construction.
    public void BuildBarracks()
    {
        TryBuild(_barracksPrefabEntity, BarracksCost);
    }

    void TryBuild(Entity prefab, int cost)
    {
        if (prefab == Entity.Null || BaseRef == null)
            return;

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
