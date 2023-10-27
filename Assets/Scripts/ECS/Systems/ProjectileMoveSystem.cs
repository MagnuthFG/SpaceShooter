using Unity.Entities;
using Unity.Transforms;
using Unity.Collections;

namespace SpaceShooter.ECS
{
    [DisableAutoCreation]
    [UpdateAfter(typeof(ProjectileSpawnSystem))]
    public partial class ProjectileMoveSystem : SystemBase
    {
        private EntityManager _manager   = default;
        private EntityQuery _bulletQuery = default;

// INITIALISATION

        protected override void OnCreate(){
            base.OnCreate();

            var world = World.DefaultGameObjectInjectionWorld;
            _manager  = world.EntityManager;

            _manager.AddComponentData(SystemHandle,
                new MoveSettingsComponent(){
                    Speed  = 6,
                    YLimit = 7.68f
                }
            );
            _bulletQuery = new EntityQueryBuilder(Allocator.Temp)
                .WithAll<ProjectileTag>()
                .WithAll<SpawnedTag>()
                .WithAll<LocalTransform>()
                .Build(this);
        }

// PROJECTILE MOVEMENT

        protected override void OnUpdate(){
            var bullets = _bulletQuery.ToEntityArray(Allocator.Temp);
            if (bullets.Length == 0) return;

            var settings = _manager.GetComponentData
                <MoveSettingsComponent>(SystemHandle);

            var transforms = _bulletQuery.ToComponentDataArray
                <LocalTransform>(Allocator.Temp);

            var deltaTime = SystemAPI.Time.DeltaTime;

            for (var i = 0; i < bullets.Length; i++){
                var bullet    = bullets[i];
                var transform = transforms[i];

                transform.Position.y += settings.Speed * deltaTime;

                if (transform.Position.y > settings.YLimit){
                    transform.Position.y = 0;
                    transform.Position.z = 2.56f;
                    _manager.RemoveComponent<SpawnedTag>(bullet);
                }
                _manager.SetComponentData(bullet, transform);
            }
        }

    }
}