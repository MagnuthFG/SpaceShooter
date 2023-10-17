using UnityEngine;

namespace SpaceShooter.Mono
{
    public class ProjectileMove : MonoBehaviour
    {
        private Transform _transform = null;

// INITIALISATION

        private void Awake(){
            _transform = transform;
        }

// MOVING

        public void Fire(Vector3 position){
            _transform.position = position;
        }
    }
}