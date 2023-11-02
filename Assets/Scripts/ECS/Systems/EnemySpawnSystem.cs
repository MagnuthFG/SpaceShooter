using Unity.Entities;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;
using Unity.Burst;

namespace SpaceShooter.ECS
{
    [DisableAutoCreation][BurstCompile]
    public partial class EnemySpawnSystem : SystemBase
    {
        private EntityManager _manager    = default;
        private EntityQuery _poolQuery    = default;
        private EntityQuery _spawnedQuery = default;

        private float3 _position = new float3(0, 0, 2.56f);
        private float3 _rotation = new float3(0, 0, 90 * Mathf.Deg2Rad);
        private EnemyFactory _factory = null;

// INITIALISATION

        [BurstCompile]
        protected override void OnCreate(){
            base.OnCreate();

            var world = World.DefaultGameObjectInjectionWorld;
            _manager  = world.EntityManager;

            _manager.AddComponentData(SystemHandle, 
                new PoolSettingsComponent(){
                    GrowthCount = 20,
                    MaxCount    = 1000
                }
            );
            _poolQuery = new EntityQueryBuilder(Allocator.Temp)
                .WithAll<EnemyTag>()
                .WithNone<SpawnedTag>()
                .Build(this);

            _spawnedQuery = new EntityQueryBuilder(Allocator.Temp)
                .WithAll<EnemyTag>()
                .WithAll<SpawnedTag>()
                .Build(this);
        }

        [BurstCompile]
        protected override void OnStartRunning(){
            var resources = ResourceManager.Instance;

            _factory = new EnemyFactory(
                resources.QuadMesh,
                resources.EnemyMaterial
            );
            var settings = _manager.GetComponentData
                <PoolSettingsComponent>(SystemHandle);

            for (int i = 0; i < settings.GrowthCount; i++){
                _factory.Create(_position, _rotation);
            }
        }

// ENEMY SPAWNING

        [BurstCompile]
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