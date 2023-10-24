using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace SpaceShooter.ECS
{
    public partial class PlayerSpawnSystem : SystemBase
    {
        protected override void OnUpdate(){
            var resources = ResourceManager.Instance;

            var factory = new PlayerFactory(
                resources?.QuadMesh,
                resources?.PlayerMaterial
            );
            factory.Create(
                new float3(0, -5.12f, 0),
                new float3(0, 0, 90 * Mathf.Deg2Rad)
            );
            // Spawn HULL as well
            Enabled = false;
        }
    }
}