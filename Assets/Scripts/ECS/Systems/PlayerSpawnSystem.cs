using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using Unity.Burst;

namespace SpaceShooter.ECS
{
    [DisableAutoCreation][BurstCompile]
    public partial class PlayerSpawnSystem : SystemBase
    {
        [BurstCompile]
        protected override void OnStartRunning(){
            var resources = ResourceManager.Instance;

            var playerFactory = new PlayerFactory(
                resources.QuadMesh,
                resources.PlayerMaterial
            );
            playerFactory.Create(
                new float3(0, -5.12f, 0),
                new float3(0, 0, 90 * Mathf.Deg2Rad)
            );
            var hullFactory = new HullFactory(
                resources.QuadMesh,
                resources.HullMaterial
            );
            hullFactory.Create(
                new float3(0, -7.08f, 0),
                new float3(0, 0, 90 * Mathf.Deg2Rad)
            );
            Enabled = false;
        }

        protected override void OnUpdate(){}// not running
    }
}