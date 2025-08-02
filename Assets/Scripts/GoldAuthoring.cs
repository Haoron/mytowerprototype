// GoldAuthoring.cs
using Unity.Entities;
using UnityEngine;

public class GoldAuthoring : MonoBehaviour
{
    public int StartingGold = 200;

    public class Baker : Baker<GoldAuthoring>
    {
        public override void Bake(GoldAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.None);
            AddComponent(entity, new Gold { Amount = authoring.StartingGold });
        }
    }
}
