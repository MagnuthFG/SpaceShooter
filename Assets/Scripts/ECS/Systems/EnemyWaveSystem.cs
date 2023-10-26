using Unity.Entities;
using Unity.Transforms;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;

namespace SpaceShooter.ECS
{
    [UpdateAfter(typeof(EnemySpawnSystem))]
    public partial class EnemyWaveSystem : SystemBase
    {
        private EntityManager _manager    = default;
        private EntityQuery _pooledQuery  = default;
        private EntityQuery _spawnedQuery = default;

// INITIALISATION

        protected override void OnCreate(){
            base.OnCreate();

            var world = World.DefaultGameObjectInjectionWorld;
            _manager  = world.EntityManager;

            var settings = new WaveSettingsComponent(){
                LaneWidth = 2.56f,
                FieldExtent = 10.24f,
                YStart = 7.68f,
                MinMaxOffset = new float2(1.6f, 2.56f),

                StartCount = 1,
                Increase = 0.675f,
            };
            int stacks = (int)((settings.FieldExtent * 2) / settings.LaneWidth);

            var info = new WaveInfoComponent(){
                Stacks = new NativeArray<int>(new int[stacks], Allocator.Temp)
            };
            _manager.AddComponentData(SystemHandle, settings);
            _manager.AddComponentData(SystemHandle, info);

            _pooledQuery = new EntityQueryBuilder(Allocator.Temp)
                .WithAll<EnemyTag>()
                .WithNone<SpawnedTag>()
                .Build(this);

            _spawnedQuery = new EntityQueryBuilder(Allocator.Temp)
                .WithAll<EnemyTag>()
                .WithAll<SpawnedTag>()
                .WithAll<LocalTransform>()
                .Build(this);
        }

// ENEMY WAVES

        protected override void OnUpdate(){
            // Check how many enemies there are alive
            // If the count is greater than 0 then return

            // Increment wave spawn count
            // clamp count based on how many there are available

            // Loop over pooled enemies
            // Calculate and set random positions 
            // Set Y Offset based on stack count
            // Add spawned tag

            var info = _manager.GetComponentData
                <WaveInfoComponent>(SystemHandle);

            
        }

    }
}
