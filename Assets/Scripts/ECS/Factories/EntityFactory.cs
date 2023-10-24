using Unity.Entities;
using Unity.Rendering;
using Unity.Transforms;
using Unity.Mathematics;
using UnityEngine.Rendering;
using UnityEngine;

namespace SpaceShooter.ECS
{
    public abstract class EntityFactory : IEntityFactory
    {
        protected World _world = null;
        protected EntityManager _manager = default;

        protected LocalTransform _transform = default;
        private readonly RenderMeshArray _meshArray = default;
        private readonly RenderBounds _renderBounds = default;
        private readonly RenderMeshDescription _meshDescription = default;

        public EntityFactory(Mesh mesh, Material material){
            _world   = World.DefaultGameObjectInjectionWorld;
            _manager = _world.EntityManager;

            _meshArray = new RenderMeshArray(
                new Material[]{ material },
                new Mesh[]{ mesh }
            );
            _renderBounds = new RenderBounds(){
                Value = mesh.bounds.ToAABB()
            };
            _meshDescription = new RenderMeshDescription(
                ShadowCastingMode.Off, false
            );
        }

        public virtual Entity Create(float3 position, float3 euler){
            var entity = _manager.CreateEntity();

            var transform = _transform;
                transform.Position = position;
                transform.Rotation = quaternion.Euler(euler);

            RenderMeshUtility.AddComponents(
                entity, _manager, _meshDescription, _meshArray,
                MaterialMeshInfo.FromRenderMeshArrayIndices(0, 0)
            );
            _manager.AddComponent<LocalToWorld>(entity);
            _manager.SetComponentData(entity, _renderBounds);

            _manager.AddComponent<LocalTransform>(entity);
            _manager.SetComponentData(entity, transform);

            return entity;
        }
    }
}