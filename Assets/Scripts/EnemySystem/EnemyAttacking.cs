using Configs;
using Enums;
using Extentions;
using Player;
using ShootingSystem;
using UnityEngine;

namespace EnemySystem
{
    public class EnemyAttacking : MonoBehaviour
    {
        [SerializeField] private EnemyView _enemyView;
        [SerializeField] private int _playerLayerIndex;
        [SerializeField] private int _playerRagdolLayerIndex;
        [SerializeField] private int _projectileLayerIndex;
        [SerializeField] private Transform _projectileSpawnTransform;

        private GameObject _projectilePrefab;
        private GameObject _projectilesSpawnRoot;
        private PlayerView _playerView;
        private EnemyConfig _enemyConfig;
        private EnemyMovement _enemyMovement;
        private float _lastAttackTime;
        private bool _canAttack;
        private bool _isAttacking;

        public GameObject ProjectilePrefab
        {
            get => _projectilePrefab;
            set => _projectilePrefab = value;
        }

        public GameObject ProjectilesSpawnRoot
        {
            get => _projectilesSpawnRoot;
            set => _projectilesSpawnRoot = value;
        }

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
            float distance = Vector3.Distance(_playerView.PlayerDamagableZoneTransform.position, 
                gameObject.transform.position);
            RaycastHit hitPoint;
            LayerMask layerMask;
            layerMask = 1 << _projectileLayerIndex;
            layerMask = ~layerMask;
            Vector3 direction = (_playerView.PlayerTransform.position - gameObject.transform.position).normalized;
            bool isHitCanMadeImpact = Physics.Raycast(gameObject.transform.position,
                direction, out hitPoint, _enemyConfig.AttackDistance, layerMask);
            if (isHitCanMadeImpact)
            {
                if (hitPoint.collider.gameObject.layer != _playerLayerIndex &&
                    hitPoint.collider.gameObject.layer != _playerRagdolLayerIndex)
                {
                    isHitCanMadeImpact = false;
                }
            }

            _canAttack = (Time.time > _lastAttackTime + _enemyConfig.EnemyAttackDelay) &&
                         distance < _enemyConfig.AttackDistance && isHitCanMadeImpact && !_isAttacking && !_enemyView.IsDead;
            if (_canAttack)
            {
                Debug.Log("StartAttack");
                _lastAttackTime = Time.time;
                _isAttacking = true;
                if (_enemyConfig.AttackType == EnemyAttackType.Shoot)
                {
                    _enemyMovement.ChangeMovementBehaviour(MovementBehaviour.Standing);
                }
            }
            else if (_enemyConfig.AttackType == EnemyAttackType.Shoot && !isHitCanMadeImpact)
            {
                _enemyMovement.ChangeMovementBehaviourToDefault();
            }

            if (!isHitCanMadeImpact)
            {
                _isAttacking = false;
            }

            if (_isAttacking && Time.time > _lastAttackTime + _enemyConfig.EnemyAttackDuration)
            {
                if (!_enemyView.IsDead)
                {
                    Attack();
                }
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
                _projectileSpawnTransform.LookAt(_playerView.PlayerTransform);
                GameObject projectile = GameObject.Instantiate(_projectilePrefab,
                    _projectileSpawnTransform.position ,_projectileSpawnTransform.rotation, _projectilesSpawnRoot.transform);
                Projectile projectileView = projectile.GetOrAddComponent<Projectile>();
                projectileView.StartMoving(_projectileSpawnTransform.position, 
                    _playerView.PlayerDamagableZoneTransform.position,
                    gameObject.transform);
            }
        
        }
    }
}
