using SF = UnityEngine.SerializeField;
using UnityEngine.InputSystem;
using UnityEngine;

namespace SpaceShooter.Mono
{
    public class PlayerStrafe : MonoBehaviour
    {
        [SF] private float _stepDistance = 2.56f;
        [SF] private float _maxDistance  = 10.24f;
        [SF] private InputActionReference _input = null;

        private Transform _transform = null;

// INITIALISATION

        private void Awake(){
            _transform = transform;
        }

        private void OnEnable(){
            _input.action.started += ctx => OnInput(ctx);
            _input.action.Enable();
        }
        
        private void OnDisable(){
            _input.action.started -= ctx => OnInput(ctx);
            _input.action.Disable();
        }

// MOVEMENT

        private void OnInput(InputAction.CallbackContext ctx){
            var direction = ctx.ReadValue<float>();
            var position  = _transform.position;

            position.x = Mathf.Clamp(
                position.x + (_stepDistance * direction), 
                -_maxDistance, _maxDistance
            );
            _transform.position = position;
        }

    }
}