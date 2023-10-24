using SF = UnityEngine.SerializeField;
using UnityEngine;

namespace SpaceShooter.ECS
{
    [DefaultExecutionOrder(-10)]
    public class ResourceManager : MonoBehaviour
    {
        private static ResourceManager s_instance = null;

        [SF] private Mesh _quadMesh = null;
        [SF] private Material _playerMaterial     = null;
        [SF] private Material _enemyMaterial      = null;
        [SF] private Material _projectileMaterial = null;
        [SF] private Material _hullMaterial       = null;

// PROPERTIES

        public static ResourceManager Instance { 
            get => s_instance;

            private set => s_instance = value; 
        }
        
        public Mesh     QuadMesh           => _quadMesh;
        public Material PlayerMaterial     => _playerMaterial;
        public Material EnemyMaterial      => _enemyMaterial;
        public Material ProjectileMaterial => _projectileMaterial;
        public Material HullMaterial       => _hullMaterial;

// INITIALISATION

        private void Awake() => Instance = this;

    }
}