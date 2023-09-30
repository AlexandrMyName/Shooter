using Configs;
using Enums;
using EventBus;
using Extentions;
using Player;
using RootMotion.Dynamics;
using UnityEngine;


namespace EnemySystem
{
    
    public class EnemyView : MonoBehaviour
    {
        
        [SerializeField] private EnemyConfig _enemyConfig;
        [SerializeField] private EnemyMovement _enemyMovement;
        [SerializeField] private EnemyAttacking _enemyAttacking;
        [SerializeField] private PlayerView _playerView;
        [SerializeField] private PuppetMaster _puppetMaster;
        
        private EnemyDeathSystem _enemyDeath;

        private Muscle _lastHitMuscle;
        private Vector3 _lastHitProjectileDirection;
        private float _lastHitForce;
        
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


        public void TakeDamage(Rigidbody rigidBody, int damage, float force, Vector3 projectileDirection)
        {
            _lastHitProjectileDirection = projectileDirection;
            _lastHitForce = force;
            TakeForceToPuppetMuscle(rigidBody, force, projectileDirection);
            TakeDamage(damage);
        }


        private void TakeForceToPuppetMuscle(Rigidbody rigidBody, float force, Vector3 projectileDirection)
        {
            _lastHitMuscle = _puppetMaster.GetMuscle(rigidBody);
            _lastHitMuscle.props.pinWeight = 0.5f;
            _lastHitMuscle.props.muscleWeight = 0.5f;
            rigidBody.AddForce(projectileDirection * force / 2, ForceMode.Impulse);
        }

        
        private void Death()
        {
            EnemyEvents.EnemyDead();
            _isDead = true;
            _enemyMovement.StopMovement();
            AddDeadForce();
            _enemyDeath.DestroyEnemy();
        
            Debug.Log($"{gameObject.name} killed {_isDead}");
        }


        private void AddDeadForce()
        {
            _lastHitMuscle.rigidbody.AddForce(_lastHitProjectileDirection * _lastHitForce * 2, ForceMode.Impulse);
        }
        
        
    }
}
