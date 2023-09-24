using Configs;
using Enums;
using EventBus;
using Extentions;
using Player;
using UnityEngine;

namespace EnemySystem
{
    public class EnemyView : MonoBehaviour
    {
        [SerializeField] private EnemyConfig _enemyConfig;
        [SerializeField] private EnemyMovement _enemyMovement;
        [SerializeField] private EnemyAttacking _enemyAttacking;
        [SerializeField] private PlayerView _playerView;
        private EnemyDeathSystem _enemyDeath;
    
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
            _enemyDeath = GetComponent<EnemyDeathSystem>();
            _isDead = false;
            _currentEnemyHP = _enemyConfig.EnemyHp;
            if (_enemyConfig.AttackType == EnemyAttackType.Shoot)
            {
                _enemyAttacking.ProjectilePrefab = _enemyConfig.ShootingProjectile;
            }
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
            if (!_isDead)
            {
                if (Extention.CheckChance(_enemyConfig.StunPossibility))
                {
                    _isStuned = true;
                    _lastStunTime = Time.time;
                    _enemyMovement.ChangeMovementBehaviour(MovementBehaviour.Standing);
                }
                EnemyEvents.EnemyDamaged();
                EnemyHP -= damage;
            }
        }

        private void Death()
        {
            EnemyEvents.EnemyDead();
            _isDead = true;
            _enemyMovement.StopMovement();
            _enemyDeath.DestroyEnemy();
        
            Debug.Log($"{gameObject.name} killed {_isDead}");
        }
    }
}
