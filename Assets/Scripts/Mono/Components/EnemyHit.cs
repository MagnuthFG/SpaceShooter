using SF = UnityEngine.SerializeField;
using UnityEngine;

namespace SpaceShooter.Mono
{
    public class EnemyHit : MonoBehaviour
    {
        [SF] private LayerMask _damageLayer = 1 << 0;
        private GameObject _gameobject = null;

// INITIALISATION

        private void Awake(){
            _gameobject = this.gameObject;
        }

// COLLISION EVENT

        private void OnCollisionEnter2D(Collision2D collision){
            _gameobject.SetActive(false);

            if (((1 << collision.gameObject.layer) & _damageLayer) != 0)
                HealthHandler.Instance.Damage();
        }

    }
}