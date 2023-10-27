using Unity.Entities;

namespace SpaceShooter.ECS
{
    public struct StrafeSettingsComponent : IComponentData {
        public float StepDistance;
        public float MaxDistance;
    }
}