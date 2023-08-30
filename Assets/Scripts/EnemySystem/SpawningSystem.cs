using Extentions;
using RootMotion.Demos;
using UnityEngine;

public class SpawningSystem : MonoBehaviour
{
    [SerializeField] private SpawnConfig _spawnConfig;
    [SerializeField] private GameObject _spawnableEnemyPrefab;
    [SerializeField] private GameObject _goalObjectPrefab;
    [SerializeField] private PlayerView _playerView;
    [SerializeField] private GameObject _spawnedObjectsRoot;
    [SerializeField] private GameObject _goalObjectsRoot;

    private int _maxSpawnPointIndex;
    private int _currentSpawnPointIndex;
    private int _spawnedInCurrentWave;
    private int _totalSpawned;
    private float _lastSpawnTime;
    private float _lastWaveTime;
    private bool _isWaveSpawning;

    private void Start()
    {
        _maxSpawnPointIndex = _spawnConfig.SpawnPointsList.Count - 1;
        _totalSpawned = 0;
        _spawnedInCurrentWave = 0;
        _lastWaveTime = Time.time - _spawnConfig.WaveCooldown;
        _lastSpawnTime = Time.time - _spawnConfig.SpawnCooldown;
    }

    private void FixedUpdate()
    {
        if (_totalSpawned < _spawnConfig.TotalSpawnCount)
        {
            TrySpawn();
        }
    }

    private void TrySpawn()
    {
        if (Time.time > _lastWaveTime + _spawnConfig.WaveCooldown && !_isWaveSpawning)
        {
            _isWaveSpawning = true;
            _lastWaveTime = Time.time;
            _spawnedInCurrentWave = 0;
        }
        
        if (Time.time > _lastSpawnTime + _spawnConfig.SpawnCooldown && _isWaveSpawning)
        {
            _lastSpawnTime = Time.time;
            _currentSpawnPointIndex = Extention.GetRandomInt(0, _maxSpawnPointIndex);
            gameObject.transform.position = _spawnConfig.SpawnPointsList[_currentSpawnPointIndex];
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
        GameObject enemy = GameObject.Instantiate(_spawnableEnemyPrefab,
            gameObject.transform.position,
            gameObject.transform.rotation,
            _spawnedObjectsRoot.transform);
        GameObject goal = GameObject.Instantiate(_goalObjectPrefab,
            gameObject.transform.position,
            gameObject.transform.rotation,
            _goalObjectsRoot.transform);
        enemy.TryGetComponent(out EnemyView enemyView);
        enemyView.PlayerView = _playerView;
        enemy.TryGetComponent(out EnemyMovement enemyMovement);
        enemyMovement.GoalObject = goal;
        enemy.TryGetComponent(out NavMeshPuppet navMeshPuppet);
        navMeshPuppet.target = goal.transform;
    }
}
