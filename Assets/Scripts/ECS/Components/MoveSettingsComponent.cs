using Unity.Entities;

namespace SpaceShooter.ECS
{
    public struct MoveSettingsComponent : IComponentData {
        public float Speed;
        public float Torque;
        public float YLimit;
    }
}