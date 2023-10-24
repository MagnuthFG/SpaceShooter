using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace SpaceShooter.ECS
{
    public class HullFactory : EntityFactory
    {
        public HullFactory(Mesh mesh, Material material) : base(mesh, material){

        }

        public override Entity Create(float3 position, float3 euler){
            var entity = base.Create(position, euler);

            // Add collision

            _manager.AddComponent<PlayerTag>(entity);
            _manager.SetName(entity, "Hull");

            return entity;
        }
    }
}