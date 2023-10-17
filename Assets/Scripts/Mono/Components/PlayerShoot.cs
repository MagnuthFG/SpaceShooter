using SF = UnityEngine.SerializeField;
using UnityEngine.InputSystem;
using UnityEngine;

namespace SpaceShooter.Mono
{
    public class PlayerShoot : MonoBehaviour
    {
        [SF] private float _delay = 0.1f;
        [SF] private Vector3 _offset = new Vector3(0, 0.2f, 0);
        [SF] private GameObject _projectile = null;
        [SF] private InputActionReference _input = null;

        private float _nextTime = 0;
        private Transform _transform = null;
        private ComponentPool<ProjectileMove> _pool = null;

// INITIALISATION

        private void Awake(){
            _transform = transform;
            
            _pool = new ComponentPool<ProjectileMove>(
                _projectile, 5, 10
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

// SHOOTING

        private void OnInput(InputAction.CallbackContext ctx){
            var time = Time.time;
            
            if (time < _nextTime) return;
            _nextTime = time + _delay;

            var projectile = _pool.Get();
                projectile?.gameObject.SetActive(true);
                projectile?.Fire(_transform.position + _offset);
        }

    }
}