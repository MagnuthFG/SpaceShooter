using UnityEngine.InputSystem;
using Unity.Entities;
using Unity.Transforms;
using Unity.Collections;

namespace SpaceShooter.ECS
{
    [UpdateAfter(typeof(ProjectileSpawnSystem)),
     UpdateAfter(typeof(PlayerSpawnSystem))]
    public partial class PlayerShootSystem : SystemBase
    {
        private EntityManager _manager   = default;
        private EntityQuery _playerQuery = default;
        private EntityQuery _bulletQuery = default;

        private InputActionReference _shootInput = null;

// INITIALISATION

        protected override void OnCreate(){
            base.OnCreate();

            var world = World.DefaultGameObjectInjectionWorld;
            _manager  = world.EntityManager; 

            _playerQuery = new EntityQueryBuilder(Allocator.Temp)
                .WithAll<ShootSettingsComponent>()
                .WithAll<ShootInfoComponent>()
                .WithAll<LocalTransform>()
                .WithAll<PlayerTag>()
                .Build(this);

            _bulletQuery = new EntityQueryBuilder(Allocator.Temp)
                .WithAll<LocalTransform>()
                .WithAll<ProjectileTag>()
                .WithNone<SpawnedTag>()
                .Build(this);
        }

        protected override void OnStartRunning(){
            base.OnStartRunning();

            _shootInput = ResourceManager.Instance.ShootInput;

            _shootInput.action.started  += ctx => OnInput(ctx);
            _shootInput.action.canceled += ctx => OnInput(ctx);
            _shootInput.action.Enable();

            Enabled = false;
        }

        protected override void OnDestroy(){
            base.OnDestroy();

            _shootInput.action.started  -= OnInput;
            _shootInput.action.canceled -= OnInput;
            _shootInput.action.Disable();
        }

// SHOOT INPUT

        private void OnInput(InputAction.CallbackContext ctx){
            var players = _playerQuery.ToEntityArray(Allocator.Temp);
            var bullets = _bulletQuery.ToEntityArray(Allocator.Temp);
            var player  = players[0];
            var bullet  = bullets[0];

            var info     = _manager.GetComponentData<ShootInfoComponent>(player);
            var settings = _manager.GetComponentData<ShootSettingsComponent>(player); 
            var playerTransform = _manager.GetComponentData<LocalTransform>(player);
            var bulletTransform = _manager.GetComponentData<LocalTransform>(bullet);

            var time = (float)SystemAPI.Time.ElapsedTime;
            if (info.NextShotTime > time) return;

            info.NextShotTime = time + settings.Delay;
            _manager.SetComponentData(player, info);

            bulletTransform.Position = playerTransform.Position + settings.Offset;
            _manager.SetComponentData(bullet, bulletTransform);
            
            _manager.AddComponent<SpawnedTag>(bullet);
        }

        protected override void OnUpdate(){}// not running

    }
}
