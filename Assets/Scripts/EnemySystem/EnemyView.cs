using System;
using System.Collections;
using Configs;
using Enums;
using EventBus;
using Extentions;
using Player;
using RootMotion.Dynamics;
using UniRx;
using UnityEngine;


namespace EnemySystem
{
    
    public class EnemyView : MonoBehaviour, IComparable
    {
        
        [SerializeField] private EnemyConfig _enemyConfig;
        [SerializeField] private EnemyMovement _enemyMovement;
        [SerializeField] private EnemyAttacking _enemyAttacking;
        [SerializeField] private PlayerView _playerView;
        [SerializeField] private PuppetMaster _puppetMaster;

        [Range(0.0f, 1.0f)]
        [SerializeField] private float _pinWeightAfterCollision = 0.5f;
        
        [Range(0.0f, 1.0f)]
        [SerializeField] private float _pinMuscleAfterCollision = 0.5f;
        
        private EnemyDeathSystem _enemyDeath;

        private Muscle _lastHitMuscle;
        private Vector3 _lastHitProjectileDirection;
        private float _lastHitForce;
        
        private int _enemyID;
        private int _currentEnemyHP = 50;
        private float _lastStunTime;
        private bool _isStuned;
        private bool _isDead;
        private bool _isLastDamageExplosion;

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

        public int EnemyMaxHP => _enemyConfig.EnemyHp;
        

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
            _isLastDamageExplosion = false;
            _lastHitProjectileDirection = projectileDirection;
            _lastHitForce = force;
            RelaxMuscleBeforeForce(rigidBody, _pinWeightAfterCollision, _pinMuscleAfterCollision);
            rigidBody.AddForce(projectileDirection * force / 2, ForceMode.Impulse);
            TakeDamage(damage);
        }
        
        
        public void TakeExplosionDamage(Rigidbody rigidBody, int damage, float force, 
            Vector3 center, float radius, Vector3 projectileDirection)
        {
            _isLastDamageExplosion = true;
            _lastHitProjectileDirection = projectileDirection;
            _lastHitForce = force;
            _puppetMaster.state = PuppetMaster.State.Frozen;
            Observable.Timer(TimeSpan.FromSeconds(0.05f)).Subscribe(_ =>
            {
                rigidBody.AddExplosionForce(
                    damage,
                    center,
                    radius,
                    1000.0f,
                    ForceMode.Impulse);
                _puppetMaster.state = PuppetMaster.State.Alive;
                TakeDamage(damage);
            });
        }
        
        
        private void RelaxMuscleBeforeForce(Rigidbody rigidBody, float pinWeightAfterCollision, float pinMuscleAfterCollision)
        {
            _lastHitMuscle = _puppetMaster.GetMuscle(rigidBody);
            _lastHitMuscle.props.pinWeight = pinWeightAfterCollision;
            _lastHitMuscle.props.muscleWeight = pinMuscleAfterCollision;
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
            if (!_isLastDamageExplosion) 
                _lastHitMuscle.rigidbody.AddForce(_lastHitProjectileDirection * _lastHitForce * 2, ForceMode.Impulse);
            // else
            //     _lastHitMuscle.rigidbody.AddForce((_lastHitProjectileDirection + Vector3.up).normalized * _lastHitForce * 2, ForceMode.Impulse);
        }

        
        public int CompareTo(object obj)
        {
            if (obj is EnemyView enemyView) 
                return this.GetInstanceID().CompareTo(enemyView.GetInstanceID());
            else
                throw new ArgumentException("Incorrect object type");
        }
        
        
    }
}
