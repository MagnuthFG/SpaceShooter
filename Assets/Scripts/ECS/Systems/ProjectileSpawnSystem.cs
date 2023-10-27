using Unity.Entities;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;

namespace SpaceShooter.ECS
{
    [DisableAutoCreation]
    //[CreateAfter(typeof(PlayerSpawnSystem))]
    public partial class ProjectileSpawnSystem : SystemBase
    {
        private EntityManager _manager    = default;
        private EntityQuery _poolQuery    = default;
        private EntityQuery _spawnedQuery = default;

        private float3 _position = new float3(0, 0, 2.56f);
        private float3 _rotation = new float3(0, 0, 90 * Mathf.Deg2Rad);
        private ProjectileFactory _factory = null;

// INITIALISATION

        protected override void OnCreate(){
            base.OnCreate();

            var world = World.DefaultGameObjectInjectionWorld;
            _manager  = world.EntityManager;

            _manager.AddComponentData(SystemHandle, 
                new PoolSettingsComponent(){
                    GrowthCount = 10,
                    MaxCount    = 100
                }
            );
            _poolQuery = new EntityQueryBuilder(Allocator.Temp)
                .WithAll<ProjectileTag>()
                .WithNone<SpawnedTag>()
                .Build(this);

            _spawnedQuery = new EntityQueryBuilder(Allocator.Temp)
                .WithAll<ProjectileTag>()
                .WithAll<SpawnedTag>()
                .Build(this);
        }

        protected override void OnStartRunning(){
            var resources = ResourceManager.Instance;

            _factory = new ProjectileFactory(
                resources.QuadMesh,
                resources.ProjectileMaterial
            );
            var settings = _manager.GetComponentData
                <PoolSettingsComponent>(SystemHandle);

            for (int i = 0; i < settings.GrowthCount; i++){
                _factory.Create(_position, _rotation);
            }
        }

// PROJECTILE SPAWNING

        protected override void OnUpdate(){
            if (_poolQuery.CalculateEntityCount() > 0) return;
                
            var settings = _manager.GetComponentData
                <PoolSettingsComponent>(SystemHandle);

            var spawned = _spawnedQuery.CalculateEntityCount();
            if (spawned >= settings.MaxCount) return;

            var count = math.clamp(
                settings.GrowthCount, 
                0, settings.MaxCount - spawned
            );
            for (int i = 0; i < count; i++){
                _factory.Create(_position, _rotation);
            }
        }

    }
}