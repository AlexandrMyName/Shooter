using System;
using Configs;
using Enums;
using Extentions;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyView : MonoBehaviour
{
    [SerializeField] private EnemyConfig _enemyConfig;
    [SerializeField] private EnemyMovement _enemyMovement;
    [SerializeField] private EnemyAttacking _enemyAttacking;
    [SerializeField] private PlayerView _playerView;

    private int _currentEnemyHP = 50;
    private float _lastStunTime;
    private bool _isStuned;

    public EnemyConfig EnemyConfig => _enemyConfig;

    public EnemyMovement EnemyMovement => _enemyMovement;

    public EnemyAttacking EnemyAttacking => _enemyAttacking;

    public PlayerView PlayerView
    {
        get => _playerView;
        set => _playerView = value;
    }

    public int EnemyHP
    {
        get => _currentEnemyHP;
        set
        {
            _currentEnemyHP = value;
            if (value <= 0)
            {
                Destroy(gameObject);
                Debug.Log($"{gameObject.name} killed");
            }
        }
    }
    
    private void Start()
    {
        _currentEnemyHP = _enemyConfig.EnemyHp;
    }

    private void FixedUpdate()
    {
        if (_isStuned && Time.time > _lastStunTime + _enemyConfig.StunTime)
        {
            _isStuned = false;
            _enemyMovement.ChangeMovementBehaviourToDefault();
        }
    }


    public void TakeDamage(int damage)
    {
        if (Extention.CheckChance(_enemyConfig.StunPossibility))
        {
            _isStuned = true;
            _lastStunTime = Time.time;
            _enemyMovement.ChangeMovementBehaviour(MovementBehaviour.Standing);
        }
        EnemyHP -= damage;
        Debug.Log(EnemyHP);
    }
}
