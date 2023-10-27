using Unity.Entities;
using Unity.Transforms;
using Unity.Collections;
using Unity.Burst;

namespace SpaceShooter.ECS
{
    [DisableAutoCreation][BurstCompile]
    [UpdateAfter(typeof(EnemyWaveSystem))]
    public partial class EnemyMoveSystem : SystemBase
    {
        private EntityManager _manager  = default;
        private EntityQuery _enemyQuery = default;

// INITIALISATION

        [BurstCompile]
        protected override void OnCreate(){
            base.OnCreate();

            var world = World.DefaultGameObjectInjectionWorld;
            _manager  = world.EntityManager;

            _manager.AddComponentData(SystemHandle,
                new MoveSettingsComponent(){
                    Speed  = 3,
                    Torque = 45,
                    YLimit = -7.68f
                }
            );
            _enemyQuery = new EntityQueryBuilder(Allocator.Temp)
                .WithAll<EnemyTag>()
                .WithAll<SpawnedTag>()
                .WithAll<LocalTransform>()
                .Build(this);
        }

// ENEMY MOVEMENT

        [BurstCompile]
        protected override void OnUpdate(){
            var enemies = _enemyQuery.ToEntityArray(Allocator.Temp);
            if (enemies.Length == 0) return;

            var settings = _manager.GetComponentData
                <MoveSettingsComponent>(SystemHandle);

            var transforms = _enemyQuery.ToComponentDataArray
                <LocalTransform>(Allocator.Temp);

            var deltaTime = SystemAPI.Time.DeltaTime;

            for (var i = 0; i < enemies.Length; i++){
                var enemy     = enemies[i];
                var transform = transforms[i];

                transform.Position.y -= settings.Speed * deltaTime;
                transform.RotateZ(settings.Torque * deltaTime);

                if (transform.Position.y < settings.YLimit){
                    transform.Position.y = 0;
                    transform.Position.z = 2.56f;

                    _manager.RemoveComponent<SpawnedTag>(enemy);
                }
                _manager.SetComponentData(enemy, transform);
            }
        }

    }
}