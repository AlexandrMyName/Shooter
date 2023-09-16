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

    private int _enemyID;
    private int _currentEnemyHP = 50;
    private float _lastStunTime;
    private bool _isStuned;
    private bool _isDead;

    public EnemyConfig EnemyConfig => _enemyConfig;

    public EnemyMovement EnemyMovement => _enemyMovement;

    public EnemyAttacking EnemyAttacking => _enemyAttacking;

    public PlayerView PlayerView
    {
        get => _playerView;
        set => _playerView = value;
    }

    public int EnemyID
    {
        get => _enemyID;
        set => _enemyID = value;
    }

    public int EnemyHP
    {
        get => _currentEnemyHP;
        set
        {
            _currentEnemyHP = value;
            if (value <= 0)
            {
                Death();
            }
        }
    }

    public bool IsDead => _isDead;

    private void Start()
    {
        _isDead = false;
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
    }

    private void Death()
    {
        _isDead = true;
        _enemyMovement.StopMovement();
        
        //Destroy(gameObject);
        
        Debug.Log($"{gameObject.name} killed {_isDead}");
    }
}
