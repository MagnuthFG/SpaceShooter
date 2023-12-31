using SF = UnityEngine.SerializeField;
using UnityEngine;

namespace SpaceShooter.Mono
{
    public class ProjectileMove : MonoBehaviour
    {
        [Header("Movement Settings")]
        [SF] private float _speed = 6.0f;
        [SF] private float _maxY  = 6.4f;

        private GameObject _gameObject = null;
        private Transform  _transform  = null;

// INITIALISATION

        private void Awake(){
            _gameObject = this.gameObject;
            _transform  = this.transform;
        }

// MOVEMENT

        private void FixedUpdate(){
            var position    = _transform.position;
                position.y += _speed * Time.deltaTime;

            if (position.y > _maxY)
                _gameObject.SetActive(false);

            _transform.position = position;
        }

    }
}