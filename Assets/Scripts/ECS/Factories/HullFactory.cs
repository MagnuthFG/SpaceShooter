using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using UnityEngine;

namespace SpaceShooter.ECS
{
    public class HullFactory : EntityFactory
    {
        private PostTransformMatrix _scaleMatrix = default;

        public HullFactory(Mesh mesh, Material material) : base(mesh, material){
            _transform = new LocalTransform(){
                Scale = 1f
            };
            _scaleMatrix = new PostTransformMatrix(){
                Value = Matrix4x4.Scale(new float3(1, 25.6f, 1))
            };
        }

        public override Entity Create(float3 position, float3 euler){
            var entity = base.Create(position, euler);

            _manager.AddComponent<PostTransformMatrix>(entity);
            _manager.SetComponentData(entity, _scaleMatrix);
            
            // Add collision

            _manager.AddComponent<PlayerTag>(entity);
            _manager.SetName(entity, "Hull");

            return entity;
        }
    }
}