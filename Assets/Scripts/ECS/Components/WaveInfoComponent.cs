using Unity.Entities;
using Unity.Collections;

namespace SpaceShooter.ECS
{
    public struct WaveInfoComponent : IComponentData
    {
        public int Wave;
        public NativeArray<int> Stacks;
    }
}