using UnityEngine.InputSystem;
using Unity.Entities;
using Unity.Transforms;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;

namespace SpaceShooter.ECS
{
    public partial class PlayerShootSystem : SystemBase
    {
        private EntityManager _manager       = default;
        private EntityQuery _playerQuery     = default;
        private EntityQuery _projectileQuery = default;

        private InputActionReference _shootInput = null;

// INITIALISATION

        protected override void OnCreate(){
            base.OnCreate();

            var world = World.DefaultGameObjectInjectionWorld;
            _manager  = world.EntityManager; 

            _playerQuery = new EntityQueryBuilder(Allocator.Temp)
                .WithAll<ShootSettingsComponent>()
                .WithAll<ShootInfoComponent>()
                .WithAll<PlayerTag>()
                .Build(this);

            _projectileQuery = new EntityQueryBuilder(Allocator.Temp)
                .WithAll<ProjectileTag>()
                .WithAll<LocalTransform>()
                .WithNone<SpawnedTag>()
                .Build(this);
        }

        protected override void OnStartRunning(){
            base.OnStartRunning();

            _shootInput = ResourceManager.Instance.ShootInput;

            _shootInput.action.started  += ctx => OnInput(ctx);
            _shootInput.action.canceled += ctx => OnInput(ctx);
            _shootInput.action.Enable();
        }

        protected override void OnDestroy(){
            base.OnDestroy();

            _shootInput.action.started  -= OnInput;
            _shootInput.action.canceled -= OnInput;
            _shootInput.action.Disable();

            Enabled = false;
        }

// STRAFE INPUT

        private void OnInput(InputAction.CallbackContext ctx){
            var players     = _playerQuery.ToEntityArray(Allocator.Temp);
            var projectiles = _projectileQuery.ToEntityArray(Allocator.Temp);

            var infos = _playerQuery.ToComponentDataArray
                <ShootInfoComponent>(Allocator.Temp);

            var settings = _playerQuery.ToComponentDataArray
                <ShootSettingsComponent>(Allocator.Temp);

            var transforms = _projectileQuery.ToComponentDataArray
                <LocalTransform>(Allocator.Temp);

            var time = SystemAPI.Time.ElapsedTime;

            for (int i = 0; i < players.Length; i++){
                var player  = players[i];
                var setting = settings[i]; 
                var info    = infos[i];

                if (info.NextShotTime > time) continue;
                info.NextShotTime = (float)time + setting.Delay;
                _manager.SetComponentData(player, info);
                
                // Set projectile location
                // Add spawned tag so it starts moving

                Debug.Log("Shot Projectile");
            }
        }

        protected override void OnUpdate(){}// not running
    }
}
