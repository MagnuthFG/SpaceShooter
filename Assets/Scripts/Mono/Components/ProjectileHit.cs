using UnityEngine;

namespace SpaceShooter.Mono
{
    public class ProjectileHit : MonoBehaviour
    {
        private GameObject _gameobject = null;

// INITIALISATION

        private void Awake(){
            _gameobject = this.gameObject;
        }

// COLLISION EVENT

        private void OnCollisionEnter2D(Collision2D collision){
            _gameobject.SetActive(false);
        }

    }
}