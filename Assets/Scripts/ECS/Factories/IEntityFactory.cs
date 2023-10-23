using Unity.Entities;
using Unity.Mathematics;

namespace SpaceShooter.ECS
{
    public interface IEntityFactory
    {
        Entity Create(float3 position);
    }
}