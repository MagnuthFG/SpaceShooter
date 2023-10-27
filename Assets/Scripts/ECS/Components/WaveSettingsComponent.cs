using Unity.Entities;
using Unity.Mathematics;

namespace SpaceShooter.ECS
{
    public struct WaveSettingsComponent : IComponentData {
        public float LaneWidth;
        public float FieldExtent;
        public float YStart;
        public float2 MinMaxOffset;

        public int StartCount;
        public float Increase;
    }
}