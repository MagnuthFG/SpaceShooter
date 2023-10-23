using Unity.Entities;
using Unity.Rendering;
using Unity.Transforms;
using Unity.Mathematics;
using UnityEngine;

namespace SpaceShooter.ECS
{
    public class EnemyFactory : IEntityFactory
    {
        private World _world = null;
        private EntityManager _manager = default;

        private readonly RenderMesh _mesh = default;
        private readonly LocalTransform _transform = default;

// CONSTRUCTORS

        public EnemyFactory(Mesh mesh, Material material){
            _world   = World.DefaultGameObjectInjectionWorld;
            _manager = _world.EntityManager;

            _mesh = new RenderMesh(){
                material = material,
                mesh     = mesh
            };
            _transform = new LocalTransform(){
                Rotation = new(0, 0, 90, 0),
                Scale    = 0.75f,
            };
        }

// FACTORY

        public Entity Create(float3 position){
            var entity = _manager.CreateEntity();

            var transform = _transform;
            transform.Position = position;

            _manager.AddComponent<LocalToWorld>(entity);
            _manager.SetComponentData(entity, transform);
            _manager.SetSharedComponentManaged(entity, _mesh);



            return entity;
        }
    }
}