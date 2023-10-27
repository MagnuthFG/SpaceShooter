using Unity.Entities;

namespace SpaceShooter.ECS
{
    public struct ShootInfoComponent : IComponentData {
        public float NextShotTime;
    }
}