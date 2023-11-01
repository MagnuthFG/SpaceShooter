using Unity.Entities;
using Unity.Transforms;
using Unity.Burst;
using Unity.Jobs;

namespace SpaceShooter.ECS
{
    [DisableAutoCreation][BurstCompile]
    [UpdateAfter(typeof(EnemyWaveSystem))]
    public partial class EnemyMoveSystem : SystemBase
    {
        private World _world = null;
        private EntityManager _manager  = default;

        private EndSimulationEntityCommandBufferSystem.Singleton _esECB;

// INITIALISATION

        [BurstCompile]
        protected override void OnCreate(){
            base.OnCreate();

            _world   = World.DefaultGameObjectInjectionWorld;
            _manager = _world.EntityManager;

            _esECB = SystemAPI.GetSingleton
                <EndSimulationEntityCommandBufferSystem.Singleton>();

            _manager.AddComponentData(SystemHandle,
                new MoveSettingsComponent(){
                    Speed  = 3,
                    Torque = 45,
                    YLimit = -7.68f
                }
            );
        }

// ENEMY MOVEMENT

        [BurstCompile]
        protected override void OnUpdate(){
            var settings = _manager.GetComponentData<MoveSettingsComponent>(SystemHandle);
            var ecb = _esECB.CreateCommandBuffer(_world.Unmanaged).AsParallelWriter();
            var deltaTime = SystemAPI.Time.DeltaTime;

            Entities.WithAll<EnemyTag>().WithAll<SpawnedTag>()
            .ForEach((int entityInQueryIndex, ref LocalTransform transform, in Entity enemy) => {
                transform.Position.y -= settings.Speed * deltaTime;
                transform.RotateZ(settings.Torque * deltaTime);

                if (transform.Position.y < settings.YLimit){
                    transform.Position.y = 0;
                    transform.Position.z = 2.56f;

                    ecb.RemoveComponent<EnemyTag>(
                        entityInQueryIndex, enemy
                    );
                }
            }).ScheduleParallel();
        }

    }
}