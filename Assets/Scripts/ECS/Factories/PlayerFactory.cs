using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using UnityEngine;

namespace SpaceShooter.ECS
{
    public class PlayerFactory : EntityFactory
    {
        public PlayerFactory(Mesh mesh, Material material) : base(mesh, material){
            _transform = new LocalTransform(){
                Scale = 0.75f,
            };
        }

        public override Entity Create(float3 position, float3 euler){
            var entity = base.Create(position, euler);

            // Add collision

            _manager.AddComponent<StrafeSettingsComponent>(entity);
            _manager.AddComponent<StrafeDirectionComponent>(entity);

            _manager.AddComponent<ShootSettingsComponent>(entity);
            _manager.AddComponent<ShootInputComponent>(entity);

            _manager.AddComponent<PlayerTag>(entity);
            _manager.SetName(entity, "Player");

            return entity;
        }
    }
}