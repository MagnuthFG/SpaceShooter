using Unity.Entities;
using Unity.Mathematics;

namespace SpaceShooter.ECS
{
    public struct ShootSettingsComponent : IComponentData
    {
        public float Delay;
        public float3 Offset;
    }
}