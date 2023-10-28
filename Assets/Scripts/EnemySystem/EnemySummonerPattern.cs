using System.Collections.Generic;
using EnemySystem;
using RootMotion.Dynamics;
using UnityEngine;

public class EnemySummonerPattern : MonoBehaviour
{
    [SerializeField] private GameObject _navMeshAgent;
    [SerializeField] private GameObject _puppetMaster;
    [SerializeField] GameObject _enemyPrefab;
    [SerializeField] private bool _invincibility;
    [SerializeField] private int _count = 3;
    private SpawningSystem _spawningSystem;
    private EnemyView _enemyView;
    private bool _isSpawned = false;
    private bool _activeInvincible = false;
    private List<EnemyView> _summonedEnemies = new List<EnemyView>();

    private void Start()
    {
        _spawningSystem = FindObjectOfType<SpawningSystem>();
        _enemyView = _navMeshAgent.GetComponent<EnemyView>();
    }

    private void Update()
    {
        if (!_enemyView.IsDead && _enemyView != null)
        {
            if (_isSpawned == false && _enemyView.EnemyHP <= (_enemyView.EnemyMaxHP / 2))
            {
                SpawnEnemies();
                _isSpawned = true;
            }
            if (_invincibility && _isSpawned)
            {
                if (_summonedEnemies.Count >= 1 && _activeInvincible == false)
                {
                    _navMeshAgent.SetActive(false);
                    _puppetMaster.SetActive(false);
                    _activeInvincible = true;

                }
                else if (_summonedEnemies.Count == 0 && _activeInvincible == true)
                {
                    _navMeshAgent.SetActive(true);
                    _puppetMaster.SetActive(true);
                    _activeInvincible = false;
                }
            }

            for (int i = _summonedEnemies.Count - 1; i >= 0; i--)
            {
                EnemyView enemy = _summonedEnemies[i];
                if (enemy.IsDead)
                {
                    _summonedEnemies.RemoveAt(i);
                    _enemyView.TakeDamage(_enemyView.EnemyMaxHP / (3 * _count));
                }
            }
        }
    }

    public void SpawnEnemies()
    {
        for (int i = 0; i < _count; i++)
        {
            if (_spawningSystem != null)
            {
                Vector3 position = _navMeshAgent.transform.position;
                EnemyView bossCopyView = _spawningSystem.SpawnEnemiesForBoss(_enemyPrefab, position);
                _summonedEnemies.Add(bossCopyView);
                Debug.Log(bossCopyView);
                Debug.Log(_summonedEnemies.Count);

            }
        }
    }
}