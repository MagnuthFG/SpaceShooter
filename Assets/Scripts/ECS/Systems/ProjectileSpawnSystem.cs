using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace SpaceShooter.ECS
{
    public partial class ProjectileSpawnSystem : SystemBase
    {
        protected override void OnStartRunning(){
            var resources = ResourceManager.Instance;

            var factory = new ProjectileFactory(
                resources.QuadMesh,
                resources.ProjectileMaterial
            );
            var position = new float3(0, 0, 2.56f);
            var rotation = new float3(0, 0, 90 * Mathf.Deg2Rad);

            // Get how many it should spawn from query
            factory.Create(position, rotation);

            Enabled = false;
        }

        protected override void OnUpdate(){
            // monitor how many unspawned projectiles there are?
            // if none left, spawn more projectiles?
        }
    }
}