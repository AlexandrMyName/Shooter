using EventBus;
using Player;
using RootMotion.Demos;
using UnityEngine;

namespace EnemySystem
{
    public class BossSpawner : MonoBehaviour
    {
        [SerializeField] private SpawningSystem _spawningSystem;
        [SerializeField] private GameObject _bossPrefab;
        [SerializeField] private GameObject _goalObjectPrefab;
        [SerializeField] private GameObject _spawnPoint;
        [SerializeField] private GameObject _spawnedObjectsRoot;
        [SerializeField] private GameObject _goalObjectsRoot;
        [SerializeField] private float _timeToSpawn;
        [SerializeField] private bool _isSpawningByTime;

        private PlayerView _playerView;
        private bool _isSpawnerActive;
        private bool _isSpawned;
        private bool _isKilled;
        private EnemyView _enemyView;
        private float _startTime;


        private void Start()
        {
            _playerView = _spawningSystem.PlayerView;
            _startTime = Time.time;
        }

        private void Update()
        {
            if (_isSpawnerActive && !_isSpawned && Input.GetKeyDown(KeyCode.E) && !_isSpawningByTime)
            {
                ActivteBossFight();
            }
        }

        private void FixedUpdate()
        {
            if (_enemyView != null)
            {
                Debug.Log(_enemyView.IsDead);
                if (_enemyView.IsDead)
                {
                    _enemyView = null;
                    Debug.Log("GetKey");
                    PlayerEvents.ChangeKeyStatus(true);
                }
            }

            if (!_isSpawned && _isSpawningByTime)
            {
                _timeToSpawn -= Time.fixedDeltaTime;
                if (_startTime - _timeToSpawn >= 0 && !_isSpawned)
                {
                    ActivteBossFight();
                }
            }
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.TryGetComponent(out PlayerBoneView playerBoneView))
            {
                _isSpawnerActive = true;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.TryGetComponent(out PlayerBoneView playerBoneView))
            {
                _isSpawnerActive = false;
            }
        }

        private void ActivteBossFight()
        {
            _isSpawned = true;
            _spawningSystem.gameObject.SetActive(false);
            GameObject enemy = GameObject.Instantiate(_bossPrefab,
                _spawnPoint.transform.position,
                _spawnPoint.transform.rotation,
                _spawnedObjectsRoot.transform);
            GameObject goal = GameObject.Instantiate(_goalObjectPrefab,
                gameObject.transform.position,
                gameObject.transform.rotation,
                _goalObjectsRoot.transform);
            enemy.TryGetComponent(out EnemyMovement enemyMovement);
            EnemyView enemyView = enemyMovement.EnemyView;
            _enemyView = enemyView;
            enemyView.EnemyID = _spawningSystem.CurrentID;
            _spawningSystem.CurrentID++;
            enemyView.PlayerView = _playerView;
            enemyMovement.GoalObject = goal;
            NavMeshPuppet navMeshPuppet = enemyMovement.NavPuppet;
            navMeshPuppet.target = goal.transform;
        }
    }
}
