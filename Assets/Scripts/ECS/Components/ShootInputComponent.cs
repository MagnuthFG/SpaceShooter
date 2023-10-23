using Unity.Entities;

namespace SpaceShooter.ECS
{
    public struct ShootInputComponent : IComponentData
    {
        public bool Value;
    }
}