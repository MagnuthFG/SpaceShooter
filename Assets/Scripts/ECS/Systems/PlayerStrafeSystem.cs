using UnityEngine.InputSystem;
using Unity.Entities;
using Unity.Transforms;
using Unity.Collections;
using Unity.Mathematics;

namespace SpaceShooter.ECS
{
    [DisableAutoCreation]
    [UpdateAfter(typeof(PlayerSpawnSystem))]
    public partial class PlayerStrafeSystem : SystemBase
    {
        private EntityManager _manager = default;
        private EntityQuery   _query   = default;

        private InputActionReference _strafeInput = null;

// INITIALISATION

        protected override void OnCreate(){
            base.OnCreate();

            var world = World.DefaultGameObjectInjectionWorld;
            _manager  = world.EntityManager; 

            _query = new EntityQueryBuilder(Allocator.Temp)
                .WithAll<StrafeSettingsComponent>()
                .WithAllRW<LocalTransform>()
                .WithAll<PlayerTag>()
                .Build(this);
        }

        protected override void OnStartRunning(){
            base.OnStartRunning();

            _strafeInput = ResourceManager.Instance.StrafeInput;

            _strafeInput.action.started  += ctx => OnInput(ctx);
            _strafeInput.action.canceled += ctx => OnInput(ctx);
            _strafeInput.action.Enable();

            Enabled = false;
        }

        protected override void OnDestroy(){
            base.OnDestroy();

            _strafeInput.action.started  -= OnInput;
            _strafeInput.action.canceled -= OnInput;
            _strafeInput.action.Disable();
        }

// STRAFE INPUT

        private void OnInput(InputAction.CallbackContext ctx){
            var players = _query.ToEntityArray(Allocator.Temp);

            var transforms = _query.ToComponentDataArray
                <LocalTransform>(Allocator.Temp);

            var settings = _query.ToComponentDataArray
                <StrafeSettingsComponent>(Allocator.Temp);

            for (int i = 0; i < players.Length; i++){
                var setting   = settings[i];
                var transform = transforms[i];
                var direction = ctx.ReadValue<float>();

                transform.Position.x = math.clamp(
                    transform.Position.x + (setting.StepDistance * direction),
                    -setting.MaxDistance, setting.MaxDistance
                );
                _manager.SetComponentData(players[i], transform);
            }
        }

        protected override void OnUpdate(){}// not running
    }
}