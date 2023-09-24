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

        private PlayerView _playerView;
        private bool _isSpawnerActive;
        private bool _isSpawned;
        private bool _isKilled;
        private EnemyView _enemyView;


        private void Update()
        {
            if (_isSpawnerActive && !_isSpawned && Input.GetKeyDown(KeyCode.E))
            {
                _isSpawned = true;
                ActivteBossFight();
            }
        }

        private void FixedUpdate()
        {
            if (_enemyView != null)
            {
                if (_enemyView.IsDead)
                {
                    _enemyView = null;
                    Debug.Log("Completed");
                }
            }
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.TryGetComponent(out PlayerBoneView playerBoneView))
            {
                _playerView = playerBoneView.PlayerView;
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
