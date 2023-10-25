using SF = UnityEngine.SerializeField;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SpaceShooter.ECS
{
    [DefaultExecutionOrder(-10)]
    public class ResourceManager : MonoBehaviour
    {
        private static ResourceManager s_instance = null;

        [Header("Visual Resources")]
        [SF] private Mesh _quadMesh = null;
        [SF] private Material _playerMaterial     = null;
        [SF] private Material _enemyMaterial      = null;
        [SF] private Material _projectileMaterial = null;
        [SF] private Material _hullMaterial       = null;

        [Header("Input Resources")]
        [SF] private InputActionReference _strafeInput = null;
        [SF] private InputActionReference _shootInput  = null;

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

        public InputActionReference StrafeInput => _strafeInput;
        public InputActionReference ShootInput  => _shootInput;

// INITIALISATION

        private void Awake() => Instance = this;

    }
}