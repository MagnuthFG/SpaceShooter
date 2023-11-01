using Unity.Entities;
using Unity.Burst;
using SpaceShooter.Mono;

namespace SpaceShooter.ECS
{
    [DisableAutoCreation][BurstCompile]
    [UpdateAfter(typeof(EnemyHitSystem))]
    public partial class EnemyDamageSystem : SystemBase
    {
        private EntityManager _manager   = default;

// INITIALISATION

        [BurstCompile]
        protected override void OnCreate(){
            base.OnCreate();

            var world = World.DefaultGameObjectInjectionWorld;
            _manager  = world.EntityManager;
        }

// PROJECTILE DAMAGE

        [BurstCompile]
        protected override void OnUpdate(){
            var health = HealthHandler.Instance;

            Entities.WithAll<EnemyTag>().WithAll<DamageTag>()
            .ForEach((in Entity enemy) => {
                _manager.RemoveComponent<DamageTag>(enemy);
                health.Damage();

            }).WithStructuralChanges().Run();
        }
    }
}