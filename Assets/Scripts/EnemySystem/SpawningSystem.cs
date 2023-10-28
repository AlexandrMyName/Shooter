using System;
using System.Collections.Generic;
using Configs;
using Extentions;
using Player;
using RootMotion.Demos;
using UnityEngine;
using UnityEngine.AI;

namespace EnemySystem
{
    public class SpawningSystem : MonoBehaviour
    {
        [SerializeField] private SpawnConfig _spawnConfig;
        [SerializeField] private GameObject _goalObjectPrefab;
        [SerializeField] private PlayerView _playerView;
        [SerializeField] private GameObject _spawnedObjectsRoot;
        [SerializeField] private GameObject _goalObjectsRoot;
        [SerializeField] private GameObject _projectilesSpawnRoot;
        [SerializeField] private List<GameObject> _spawnableEnemyPrefabs;

        private int _currentSpawnPointIndex;
        private int _spawnedInCurrentWave;
        private int _totalSpawned;
        private int _currentID;
        private float _lastSpawnTime;
        private float _lastWaveTime;
        private bool _isWaveSpawning;
        private List<SpawnerView> _currentlyActiveSpawners;

        public PlayerView PlayerView => _playerView;

        public int CurrentID
        {
            get => _currentID;
            set => _currentID = value;
        }

        private void Awake()
        {
            _currentlyActiveSpawners = new List<SpawnerView>();
        }

        private void Start()
        {
            _totalSpawned = 0;
            _spawnedInCurrentWave = 0;
            _currentID = 0;
            _lastWaveTime = Time.time - _spawnConfig.WaveCooldown + 1;
            _lastSpawnTime = Time.time - _spawnConfig.SpawnCooldown + 1;
        }

        private void FixedUpdate()
        {
            if (_totalSpawned < _spawnConfig.TotalSpawnCount)
            {
                TrySpawn();
            }
        }

        public void AddNewSpawnPoints(List<SpawnerView> newSpawnersList)
        {
            foreach (SpawnerView spawnerView in newSpawnersList)
            {
                _currentlyActiveSpawners.Add(spawnerView);
            }
        }

        public void ClearActiveSpawners()
        {
            _currentlyActiveSpawners.Clear();
        }

        private void TrySpawn()
        {
            if (Time.time > _lastWaveTime + _spawnConfig.WaveCooldown && !_isWaveSpawning)
            {
                _isWaveSpawning = true;
                _lastWaveTime = Time.time;
                _spawnedInCurrentWave = 0;
            }

            if (Time.time > _lastSpawnTime + _spawnConfig.SpawnCooldown && _isWaveSpawning &&
                _currentlyActiveSpawners.Count > 0)
            {
                _lastSpawnTime = Time.time;
                if (_currentlyActiveSpawners.Count > 1)
                {
                    _currentSpawnPointIndex = Extention.GetRandomInt(0, _currentlyActiveSpawners.Count);
                }
                else
                {
                    _currentSpawnPointIndex = 0;
                }
                gameObject.transform.position = _currentlyActiveSpawners[_currentSpawnPointIndex].SpawnerPosition;
                Spawn();
                _spawnedInCurrentWave++;
                _totalSpawned++;
            }

            if (_spawnedInCurrentWave >= _spawnConfig.WaveSpawnCount)
            {
                _isWaveSpawning = false;
            }
        }

        private void Spawn()
        {
            GameObject prefab = _spawnableEnemyPrefabs[Extention.GetRandomInt(0, _spawnableEnemyPrefabs.Count)];
            GameObject enemy = GameObject.Instantiate(prefab,
                gameObject.transform.position,
                gameObject.transform.rotation,
                _spawnedObjectsRoot.transform);
            GameObject goal = GameObject.Instantiate(_goalObjectPrefab,
                gameObject.transform.position,
                gameObject.transform.rotation,
                _goalObjectsRoot.transform);
            enemy.TryGetComponent(out EnemyMovement enemyMovement);
            EnemyView enemyView = enemyMovement.EnemyView;
            enemyView.EnemyID = _currentID;
            _currentID++;
            enemyView.PlayerView = _playerView;
            enemyView.EnemyAttacking.ProjectilesSpawnRoot = _projectilesSpawnRoot;
            enemyMovement.GoalObject = goal;
            NavMeshPuppet navMeshPuppet = enemyMovement.NavPuppet;
            navMeshPuppet.target = goal.transform;
        }
        public EnemyView SpawnEnemiesForBoss(GameObject _enemyPrefab, Vector3 bossPosition)
        {
            Vector3 copyspawnposition = SpawnEnemyPosition(bossPosition);

            GameObject bossCopy = GameObject.Instantiate(_enemyPrefab,
                copyspawnposition,
                gameObject.transform.rotation,
                _spawnedObjectsRoot.transform);
            bossCopy.TryGetComponent(out EnemyMovement bossCopyMovement);
            EnemyView bossCopyView = bossCopyMovement.EnemyView;
            bossCopyView.EnemyID = CurrentID;
            CurrentID++;
            bossCopyView.PlayerView = _playerView;
            bossCopyView.EnemyAttacking.ProjectilesSpawnRoot = _projectilesSpawnRoot;
            bossCopyMovement.GoalObject = _goalObjectPrefab;
            NavMeshPuppet bossCopyNavMeshPuppet = bossCopyMovement.NavPuppet;
            bossCopyNavMeshPuppet.target = _goalObjectPrefab.transform;
            return bossCopyView;
        }
        public Vector3 SpawnEnemyPosition(Vector3 originalBossPosition)
        {
            Vector2 randomCirclePoint = UnityEngine.Random.insideUnitCircle.normalized * 3f;
            Vector3 spawnPosition = originalBossPosition + new Vector3(randomCirclePoint.x, 0f, randomCirclePoint.y);

            NavMeshHit navMeshHit;
            if (NavMesh.SamplePosition(spawnPosition, out navMeshHit, 3f, NavMesh.AllAreas))
            {
                Collider[] colliders = Physics.OverlapSphere(navMeshHit.position, 0.5f);
                bool canSpawn = true;
                foreach (Collider collider in colliders)
                {
                    if (collider.gameObject != _goalObjectPrefab && collider.gameObject != _spawnedObjectsRoot)
                    {
                        canSpawn = false;
                        break;
                    }
                }

                if (canSpawn)
                {
                    return navMeshHit.position;
                }
            }

            return originalBossPosition;
        }
    }
}
