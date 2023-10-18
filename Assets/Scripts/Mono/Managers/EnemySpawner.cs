using SF = UnityEngine.SerializeField;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceShooter.Mono
{
    public class EnemySpawner : MonoBehaviour
    {
        [Header("Spawner Settings")]
        [SF] private float _bucketSize   = 2.56f;
        [SF] private float _halfDistance = 7.68f;
        [SF] private float _startY       = 6.4f;
        [SF] private Vector2 _minMaxRowOffset = new Vector2(1.6f, 2.56f);

        [Header("Wave Settings")]
        [SF] private int _startCount = 1;
        [SF] private float _countIncrease = 0.675f;

        [Header("Pool Settings")]
        [SF] private int _poolGrowth = 20;
        [SF] private int _poolSize   = 100;
        [SF] private GameObject _prefab = null;

        private int _wave  = 0;
        private List<int> _stacks = null;
        private ObjectPool _pool  = null;
        private List<GameObject> _spawned = null;

// INITIALISATION

        private void Awake(){
            _spawned = new List<GameObject>();
            _stacks  = new List<int>();

            int count = Mathf.RoundToInt(
                (_halfDistance * 2) / _bucketSize
            );
            for (int i = 0; i < count; i++){
                _stacks.Add(0);
            }
            _pool = new ObjectPool(
                _prefab, _poolGrowth, _poolSize
            );
            _pool.Populate();
        }

// ENEMY SPAWNING

        private void Update(){
            if (_spawned.Count > 0){
                _spawned.RemoveAll(
                    enemy => !enemy.activeSelf
                );
                return;
            }
            int amount = _startCount + (int)(_countIncrease * ++_wave);

            for (int i = 0; i < amount; i++){
                var point   = Vector2.zero;
                    point.x = Random.Range(-_halfDistance, _halfDistance);
                    point.x = Mathf.Round(point.x / _bucketSize) * _bucketSize;
                    point.y = _startY;

                float pos = Mathf.InverseLerp(
                    -_halfDistance, _halfDistance, point.x
                );
                int   index = (int)(pos * (_stacks.Count - 1));
                float offset = Random.Range(_minMaxRowOffset.x, _minMaxRowOffset.y);
                point.y += offset * _stacks[index]++;

                var enemy = _pool.Get();
                    enemy.transform.position = point;
                    enemy.transform.rotation = Quaternion.Euler(0, 0, Random.value * 360);
                
                _spawned.Add(enemy);
            }
            for (int i = 0; i < _stacks.Count; i++){
                _stacks[i] = 0;
            }
        }

    }
}