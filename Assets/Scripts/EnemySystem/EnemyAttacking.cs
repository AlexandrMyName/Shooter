using Configs;
using UnityEngine;

public class EnemyAttacking : MonoBehaviour
{
    [SerializeField] private EnemyView _enemyView;

    private EnemyConfig _enemyConfig;
    private EnemyMovement _enemyMovement;
    private float _lastAttackTime;

    private void Start()
    {
        _enemyConfig = _enemyView.EnemyConfig;
        _enemyMovement = _enemyView.EnemyMovement;
        _lastAttackTime = Time.time;
    }

    private void FixedUpdate()
    {
        if (Time.time > _lastAttackTime + _enemyConfig.EnemyAttackDelay)
        {
            Attack();
        }
        
    }

    private void Attack()
    {
        _lastAttackTime = Time.time;
        Debug.Log("Attack");
    }
}
