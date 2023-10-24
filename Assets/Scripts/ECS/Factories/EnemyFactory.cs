using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using UnityEngine;

namespace SpaceShooter.ECS
{
    public class EnemyFactory : EntityFactory
    {
        public EnemyFactory(Mesh mesh, Material material) : base(mesh, material){
            _transform = new LocalTransform(){
                Scale = 1.0f,
            };
        }

        public override Entity Create(float3 position, float3 euler){
            var entity = base.Create(position, euler);

            // Add collision

            // Add enemy related components here

            _manager.AddComponent<EnemyTag>(entity);
            _manager.SetName(entity, "Enemy");

            return entity;
        }
    }
}