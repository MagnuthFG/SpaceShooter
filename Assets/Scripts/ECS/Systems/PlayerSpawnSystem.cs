using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace SpaceShooter.ECS
{
    public partial class PlayerSpawnSystem : SystemBase
    {
        protected override void OnUpdate(){
            var resources = ResourceManager.Instance;

            var playerFactory = new PlayerFactory(
                resources?.QuadMesh,
                resources?.PlayerMaterial
            );
            playerFactory.Create(
                new float3(0, -5.12f, 0),
                new float3(0, 0, 90 * Mathf.Deg2Rad)
            );
            var hullFactory = new HullFactory(
                resources?.QuadMesh,
                resources?.HullMaterial
            );
            hullFactory.Create(
                new float3(0, -7.08f, 0),
                new float3(0, 0, 90 * Mathf.Deg2Rad)
            );
            Enabled = false;
        }
    }
}