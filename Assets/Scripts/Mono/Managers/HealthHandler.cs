using SF = UnityEngine.SerializeField;
using UnityEngine;

namespace SpaceShooter.Mono
{
    public class HealthHandler : MonoBehaviour
    {
        [SF] private bool _immortal = false;
        [SF] private GameObject _gameOver = null;
        [SF] private GameObject[] _hearts = null;
        
        private int _health = 0;

// PROPERTIES

        public static HealthHandler Instance { get; private set; }

// INITIALISATION

        private void Awake(){
            Instance = this;

            _health = _hearts.Length;
        }

// HEALTH HANDLING

        public void Damage(){
            if (_immortal) return;

            _hearts[--_health].SetActive(false);
            if (_health > 0) return;

            _gameOver.SetActive(true);
            Time.timeScale = 0;
        }

    }
}