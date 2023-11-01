using Unity.Entities;
using Unity.Rendering;
using Unity.Collections;
using Unity.Transforms;
using Unity.Burst;

namespace SpaceShooter.ECS
{
    [DisableAutoCreation][BurstCompile]
    [UpdateAfter(typeof(EnemyMoveSystem))]
    public partial class EnemyHitSystem : SystemBase
    {
        private World _world = null;
        private EntityManager _manager   = default;
        private EntityQuery _playerQuery = default;

        private EndSimulationEntityCommandBufferSystem.Singleton _esECB;

// INITIALISATION

        [BurstCompile]
        protected override void OnCreate(){
            base.OnCreate();

            _world = World.DefaultGameObjectInjectionWorld;
            _manager = _world.EntityManager;

            _esECB = SystemAPI.GetSingleton
                <EndSimulationEntityCommandBufferSystem.Singleton>();

            _playerQuery = new EntityQueryBuilder(Allocator.Temp)
                .WithAll<PlayerTag>()
                .WithAll<WorldRenderBounds>()
                .Build(this);
        }

// PROJECTILE DAMAGE

        [BurstCompile]
        protected override void OnUpdate() {
            var ecb = _esECB.CreateCommandBuffer(_world.Unmanaged).AsParallelWriter();

            var players = _playerQuery.ToComponentDataArray
                <WorldRenderBounds>(Allocator.TempJob);

            Entities.WithAll<EnemyTag>().WithAll<SpawnedTag>().ForEach(
                (int entityInQueryIndex, ref LocalTransform transform,
                 in WorldRenderBounds bounds, in Entity enemy) => {

                for (int i = 0; i < players.Length; i++){
                    var player = players[i];

                    if (!bounds.Value.Contains(player.Value))
                        continue;

                    transform.Position.y = 0;
                    transform.Position.z = 2.56f;

                    ecb.RemoveComponent<SpawnedTag>(
                        entityInQueryIndex, enemy
                    );
                    ecb.AddComponent<DamageTag>(
                        entityInQueryIndex, enemy
                    );
                }
            }).WithReadOnly(players)
              .WithDisposeOnCompletion(players)
              .ScheduleParallel();
        }
    }
}