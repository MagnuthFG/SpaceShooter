using Unity.Entities;
using Unity.Rendering;
using Unity.Collections;
using Unity.Transforms;
using Unity.Burst;
using SpaceShooter.Mono;

namespace SpaceShooter.ECS
{
    [DisableAutoCreation][BurstCompile]
    //[CreateAfter(typeof(EnemySpawnSystem))]
    public partial class EnemyDamageSystem : SystemBase
    {
        private EntityManager _manager   = default; 
        private EntityQuery _enemyQuery  = default;
        private EntityQuery _playerQuery = default;

// INITIALISATION

        [BurstCompile]
        protected override void OnCreate(){
            base.OnCreate();

            var world = World.DefaultGameObjectInjectionWorld;
            _manager  = world.EntityManager;

            _enemyQuery = new EntityQueryBuilder(Allocator.Temp)
                .WithAll<EnemyTag>()
                .WithAll<SpawnedTag>()
                .WithAll<WorldRenderBounds>()
                .WithAll<LocalTransform>()
                .Build(this);

            _playerQuery = new EntityQueryBuilder(Allocator.Temp)
                .WithAll<PlayerTag>()
                .WithAll<WorldRenderBounds>()
                .Build(this);
        }

// PROJECTILE DAMAGE

        [BurstCompile]
        protected override void OnUpdate(){
            var enemies = _enemyQuery.ToEntityArray(Allocator.Temp);

            var eBoundary = _enemyQuery.ToComponentDataArray
                <WorldRenderBounds>(Allocator.Temp);

            var pBoundary = _playerQuery.ToComponentDataArray
                <WorldRenderBounds>(Allocator.Temp);

            //var eTransforms = _enemyQuery.ToComponentDataArray
            //    <LocalTransform>(Allocator.Temp);

            for (int i = 0; i < eBoundary.Length; i++){
                var eBounds = eBoundary[i];
                var enemy   = enemies[i];

                for (int j = 0; j < pBoundary.Length; j++){
                    var pBounds = pBoundary[j];

                    if (!eBounds.Value.Contains(pBounds.Value)) 
                        continue;

                    //var eTransform = eTransforms[i];
                    var eTransform = _manager.GetComponentData<LocalTransform>(enemy);
                        eTransform.Position.y = 0;
                        eTransform.Position.z = 2.56f;

                    _manager.SetComponentData(enemy, eTransform);
                    _manager.RemoveComponent<SpawnedTag>(enemy);

                    HealthHandler.Instance.Damage();
                    break;
                }
            }
        }

    }
}