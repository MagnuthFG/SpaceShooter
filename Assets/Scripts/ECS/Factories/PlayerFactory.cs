using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using UnityEngine;

namespace SpaceShooter.ECS
{
    public class PlayerFactory : EntityFactory
    {
        private readonly StrafeSettingsComponent _strafeSettings = default;
        private readonly ShootSettingsComponent  _shootSettings  = default;

        public PlayerFactory(Mesh mesh, Material material) : base(mesh, material){
            _transform = new LocalTransform(){
                Scale = 0.75f,
            };
            _strafeSettings = new StrafeSettingsComponent(){
                StepDistance = 2.56f,
                MaxDistance  = 10.24f
            };
            _shootSettings = new ShootSettingsComponent(){
                Offset = new float3(0, 0.4f, 0),
                Delay  = 0.1f
            };
        }

        public override Entity Create(float3 position, float3 euler){
            var entity = base.Create(position, euler);

            // Add collision

            _manager.AddComponent<StrafeSettingsComponent>(entity);
            _manager.SetComponentData(entity, _strafeSettings);

            _manager.AddComponent<ShootInfoComponent>(entity);
            _manager.AddComponent<ShootSettingsComponent>(entity);
            _manager.SetComponentData(entity,_shootSettings);

            _manager.AddComponent<PlayerTag>(entity);
            _manager.SetName(entity, "Player");

            return entity;
        }
    }
}