using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

namespace SpaceShooter.ECS
{
    public class SystemCreator : MonoBehaviour
    {
        private List<SystemHandle> _systems = new();

        private void Awake(){
            var world = World.DefaultGameObjectInjectionWorld;

            _systems.Add(world.CreateSystem<PlayerSpawnSystem>());
            _systems.Add(world.CreateSystem<ProjectileSpawnSystem>());
            _systems.Add(world.CreateSystem<EnemySpawnSystem>());
            _systems.Add(world.CreateSystem<PlayerStrafeSystem>());
            _systems.Add(world.CreateSystem<PlayerShootSystem>());
            _systems.Add(world.CreateSystem<ProjectileMoveSystem>());
            _systems.Add(world.CreateSystem<ProjectileDamageSystem>());
            _systems.Add(world.CreateSystem<EnemyMoveSystem>());
            _systems.Add(world.CreateSystem<EnemyHitSystem>());
            _systems.Add(world.CreateSystem<EnemyDamageSystem>());
            _systems.Add(world.CreateSystem<EnemyWaveSystem>());

            var initialisation = world.GetExistingSystemManaged<InitializationSystemGroup>();
            for (int i = 0; i < 3; i++){
                initialisation.AddSystemToUpdateList(_systems[i]);
            }
            var simulation = world.GetExistingSystemManaged<SimulationSystemGroup>();
            for (int i = 3; i < _systems.Count; i++){
                simulation.AddSystemToUpdateList(_systems[i]);
            }
        }

        private void OnDestroy(){
            var world = World.DefaultGameObjectInjectionWorld;
            
            var initialisation = world.GetExistingSystemManaged<InitializationSystemGroup>();
            for (int i = 0; i < 3; i++){
                initialisation.RemoveSystemFromUpdateList(_systems[i]);
                world.DestroySystem(_systems[i]);
            }
            var simulation = world.GetExistingSystemManaged<SimulationSystemGroup>();
            for (int i = 3; i < _systems.Count; i++){
                simulation.RemoveSystemFromUpdateList(_systems[i]);
                world.DestroySystem(_systems[i]);
            }
        }
    }
}