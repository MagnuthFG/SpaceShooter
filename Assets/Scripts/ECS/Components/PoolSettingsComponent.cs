using Unity.Entities;

namespace SpaceShooter.ECS
{
    public class PoolSettingsComponent : IComponentData {
        public int GrowthCount;
        public int MaxCount;
    }
}