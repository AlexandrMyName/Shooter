using Configs;
using Enums;
using UnityEngine;

public class EnemyAttacking : MonoBehaviour
{
    [SerializeField] private EnemyView _enemyView;
    [SerializeField] private int _playerLayerIndex;

    private PlayerView _playerView;
    private EnemyConfig _enemyConfig;
    private EnemyMovement _enemyMovement;
    private float _lastAttackTime;
    private bool _canAttack;
    private bool _isAttacking;

    private void Start()
    {
        _enemyConfig = _enemyView.EnemyConfig;
        _enemyMovement = _enemyView.EnemyMovement;
        _playerView = _enemyView.PlayerView;
        _lastAttackTime = Time.time;
    }

    private void FixedUpdate()
    {
        TryAttack();
    }

    private void TryAttack()
    {
        float distance = Vector3.Distance(_playerView.PlayerTransform.position, gameObject.transform.position);
        RaycastHit hitPoint;
        LayerMask layerMask;
        layerMask = 1 << _playerLayerIndex;
        Vector3 direction = (_playerView.PlayerTransform.position - gameObject.transform.position).normalized;
        bool isHitCanMadeImpact = Physics.Raycast(gameObject.transform.position,
            direction, out hitPoint, _enemyConfig.AttackDistance, layerMask);
            
        _canAttack = (Time.time > _lastAttackTime + _enemyConfig.EnemyAttackDelay) &&
                     distance < _enemyConfig.AttackDistance && isHitCanMadeImpact && !_isAttacking;
        if (_canAttack)
        {
            Debug.Log("StartAttack");
            _lastAttackTime = Time.time;
            _isAttacking = true;
        }

        if (!isHitCanMadeImpact)
        {
            _isAttacking = false;
        }

        if (_isAttacking && Time.time > _lastAttackTime + _enemyConfig.EnemyAttackDuration)
        {
            Attack();
            _isAttacking = false;
        }

    }

    private void Attack()
    {
        
        
        if (_enemyConfig.AttackType == EnemyAttackType.Melee)
        {
            _playerView.TakeDamage(_enemyConfig.EnemyDamage);
        }
        else if (_enemyConfig.AttackType == EnemyAttackType.Shoot)
        {
            
        }
        
    }
}
