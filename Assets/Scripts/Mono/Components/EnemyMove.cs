using SF = UnityEngine.SerializeField;
using UnityEngine;

namespace SpaceShooter.Mono
{
    public class EnemyMove : MonoBehaviour
    {
        [Header("Movement Settings")]
        [SF] private float _speed  =  6.0f;
        [SF] private float _torque =  45.0f;
        [SF] private float _minY   = -6.4f;

        private GameObject _gameObject = null;
        private Transform  _transform  = null;

// INITIALISATION

        private void Awake(){
            _gameObject = this.gameObject;
            _transform  = this.transform;
        }

// MOVEMENT

        private void FixedUpdate(){
            var deltaTime = Time.deltaTime;

            var position    = _transform.position;
                position.y -= _speed * deltaTime;

            if (position.y < _minY)
                _gameObject.SetActive(false);

            var rotation    = _transform.eulerAngles;
                rotation.z -= _torque * deltaTime;

            _transform.position    = position;
            _transform.eulerAngles = rotation;
        }

    }
}