using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using UnityEngine;

namespace SpaceShooter.ECS
{
    public class ProjectileFactory : EntityFactory
    {
        public ProjectileFactory(Mesh mesh, Material material) : base(mesh, material){
            _transform = new LocalTransform(){
                Scale = 0.35f
            };
        }

        public override Entity Create(float3 position, float3 euler){
            var entity = base.Create(position, euler);

            // Add collision

            return entity;
        }
    }
}
