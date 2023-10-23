using Unity.Entities;
using UnityEngine;

namespace SpaceShooter.ECS
{
    public struct ShootSettingsComponent : IComponentData
    {
        public float Delay;
        public Vector3 Offset;
    }
}