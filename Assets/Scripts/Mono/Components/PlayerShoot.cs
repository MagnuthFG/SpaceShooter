using SF = UnityEngine.SerializeField;
using UnityEngine.InputSystem;
using UnityEngine;

namespace SpaceShooter.Mono
{
    public class PlayerShoot : MonoBehaviour
    {
        [Header("Shoot Settings")]
        [SF] private float _delay = 0.1f;
        [SF] private Vector3 _offset = new Vector3(0, 0.4f, 0);
        [SF] private InputActionReference _input = null;

        [Header("Pool Settings")]
        [SF] private int _poolGrowth = 20;
        [SF] private int _poolSize   = 100;
        [SF] private GameObject _projectile = null;

        private float _nextTime = 0;
        private Transform  _transform = null;
        private ObjectPool _pool      = null;

// INITIALISATION

        private void Awake(){
            _transform = this.transform;
            
            _pool = new ObjectPool(
                _projectile, _poolGrowth, _poolSize
            );
            _pool.Populate();
        }

        private void OnEnable(){
            _input.action.canceled += ctx => OnInput(ctx);
            _input.action.Enable();
        }
        
        private void OnDisable(){
            _input.action.canceled -= ctx => OnInput(ctx);
            _input.action.Disable();
        }

// INPUT EVENT

        private void OnInput(InputAction.CallbackContext ctx){
            var time = Time.time;
            
            if (time < _nextTime) return;
            _nextTime = time + _delay;

            var position = _transform.position + _offset;

            var projectile = _pool.Get();
                projectile.transform.position = position;
        }

    }
}