using Unity.Entities;
using Unity.Transforms;
using Unity.Collections;
using Unity.Mathematics;
using Unity.Burst;

namespace SpaceShooter.ECS
{
    [DisableAutoCreation][BurstCompile]
    [UpdateAfter(typeof(EnemySpawnSystem))]
    public partial class EnemyWaveSystem : SystemBase
    {
        private Random _random = default;

        private EntityManager _manager    = default;
        private EntityQuery _pooledQuery  = default;
        private EntityQuery _spawnedQuery = default;

// INITIALISATION

        [BurstCompile]
        protected override void OnCreate(){
            base.OnCreate();
            
            var world = World.DefaultGameObjectInjectionWorld;
            _manager  = world.EntityManager;
            _random   = Random.CreateFromIndex(0);

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
                Stacks = new NativeArray<int>(new int[stacks], Allocator.Persistent)
            };
            _manager.AddComponentData(SystemHandle, settings);
            _manager.AddComponentData(SystemHandle, info);

            _pooledQuery = new EntityQueryBuilder(Allocator.Temp)
                .WithAll<EnemyTag>()
                .WithNone<SpawnedTag>()
                .WithAll<LocalTransform>()
                .Build(this);

            _spawnedQuery = new EntityQueryBuilder(Allocator.Temp)
                .WithAll<EnemyTag>()
                .WithAll<SpawnedTag>()
                .Build(this);
        }

        [BurstCompile]
        protected override void OnDestroy(){
            base.OnDestroy();

            var info = _manager.GetComponentData
                <WaveInfoComponent>(SystemHandle);

            info.Stacks.Dispose();
        }

// ENEMY WAVES

        [BurstCompile]
        protected override void OnUpdate(){
            if (_spawnedQuery.CalculateEntityCount() > 0) return;

            var settings = _manager.GetComponentData
                <WaveSettingsComponent>(SystemHandle);

            var info = _manager.GetComponentData
                <WaveInfoComponent>(SystemHandle);

            var transforms = _pooledQuery.ToComponentDataArray
                <LocalTransform>(Allocator.Temp);

            var pooled   = _pooledQuery.ToEntityArray(Allocator.Temp);
            int increase = (int)(settings.Increase * ++info.Wave);
            int amount   = math.clamp(settings.StartCount + increase, 0, pooled.Length);

            for (int i = 0; i < amount; i++){
                var enemy     = pooled[i];
                var transform = transforms[i];

                var point   = float3.zero;
                    point.x = _random.NextFloat(-settings.FieldExtent, settings.FieldExtent);
                    point.x = math.round(point.x / settings.LaneWidth) * settings.LaneWidth;
                    point.y = settings.YStart;

                float position = math.unlerp(
                    -settings.FieldExtent, settings.FieldExtent, point.x
                );
                int   index  = (int)(position * (info.Stacks.Length - 1));
                float offset = _random.NextFloat(settings.MinMaxOffset.x, settings.MinMaxOffset.y);
                point.y += offset * info.Stacks[index]++;

                float euler = _random.NextFloat(0, 360);

                transform.Position = point;
                transform.Rotation = quaternion.EulerXYZ(0, 0, euler);

                _manager.SetComponentData(enemy, transform);
                _manager.AddComponent<SpawnedTag>(enemy);
            }
            for (int i = 0; i < info.Stacks.Length; i++){
                info.Stacks[i] = 0;
            }
            _manager.SetComponentData(SystemHandle, info);
        }

    }
}
