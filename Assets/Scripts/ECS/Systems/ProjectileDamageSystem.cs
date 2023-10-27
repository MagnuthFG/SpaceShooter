using Unity.Entities;
using Unity.Rendering;
using Unity.Collections;
using Unity.Transforms;
using Unity.Burst;

namespace SpaceShooter.ECS
{
    [DisableAutoCreation][BurstCompile]
    //[CreateAfter(typeof(ProjectileSpawnSystem))]
    public partial class ProjectileDamageSystem : SystemBase
    {
        private EntityManager _manager   = default; 
        private EntityQuery _bulletQuery = default;
        private EntityQuery _enemyQuery  = default;

// INITIALISATION

        [BurstCompile]
        protected override void OnCreate(){
            base.OnCreate();

            var world = World.DefaultGameObjectInjectionWorld;
            _manager  = world.EntityManager;

            _bulletQuery = new EntityQueryBuilder(Allocator.Temp)
                .WithAll<ProjectileTag>()
                .WithAll<SpawnedTag>()
                .WithAll<WorldRenderBounds>()
                .WithAll<LocalTransform>()
                .Build(this);

            _enemyQuery = new EntityQueryBuilder(Allocator.Temp)
                .WithAll<EnemyTag>()
                .WithAll<SpawnedTag>()
                .WithAll<WorldRenderBounds>()
                .WithAll<LocalTransform>()
                .Build(this);
        }

// PROJECTILE DAMAGE 

        [BurstCompile]
        protected override void OnUpdate(){
            var bullets = _bulletQuery.ToEntityArray(Allocator.Temp);
            var enemies = _enemyQuery.ToEntityArray(Allocator.Temp);

            var bBoundary = _bulletQuery.ToComponentDataArray
                <WorldRenderBounds>(Allocator.Temp);

            var eBoundary = _enemyQuery.ToComponentDataArray
                <WorldRenderBounds>(Allocator.Temp);

            //var bTransforms = _bulletQuery.ToComponentDataArray
            //    <LocalTransform>(Allocator.Temp);

            //var eTransforms = _enemyQuery.ToComponentDataArray
            //    <LocalTransform>(Allocator.Temp);

            for (int i = 0; i < bBoundary.Length; i++){
                var bBounds = bBoundary[i];
                var bullet  = bullets[i];

                for (int j = 0; j < eBoundary.Length; j++){
                    var eBounds = eBoundary[j];

                    if (!bBounds.Value.Contains(eBounds.Value)) 
                        continue;

                    var enemy = enemies[j];
                    //var eTransform = eTransforms[j];
                    var eTransform = _manager.GetComponentData<LocalTransform>(enemy);
                        eTransform.Position.y = 0;
                        eTransform.Position.z = 2.56f;

                    _manager.SetComponentData(enemy, eTransform);
                    _manager.RemoveComponent<SpawnedTag>(enemy);

                    //var bTransform = bTransforms[i];
                    var bTransform = _manager.GetComponentData<LocalTransform>(bullet);
                        bTransform.Position.y = 0;
                        bTransform.Position.z = 2.56f;

                    _manager.SetComponentData(bullet, bTransform);
                    _manager.RemoveComponent<SpawnedTag>(bullet);
                    break;
                }
            }
        }

    }
}