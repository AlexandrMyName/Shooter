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
        private string _bodyPart;

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
            _bodyPart = "Body";
            Vector3 bodyDirection = (_playerView.PlayerDamagableZoneTransform.position - gameObject.transform.position).normalized;
            Vector3 headDirection = (_playerView.PlayerHeadTransform.position - gameObject.transform.position).normalized;
            bool isHitCanMadeImpact = Physics.Raycast(gameObject.transform.position,
                   bodyDirection, out hitPoint, _enemyConfig.AttackDistance, layerMask);

            if (isHitCanMadeImpact && hitPoint.collider.gameObject.layer != _playerLayerIndex)
            {
                bool _isHeadHitCanMadeImpact = Physics.Raycast(gameObject.transform.position,
                    headDirection, out RaycastHit headHitPoint, _enemyConfig.AttackDistance, layerMask);
                if (_isHeadHitCanMadeImpact)
                {
                    if (headHitPoint.collider.gameObject.layer == _playerLayerIndex ||
                        headHitPoint.collider.gameObject.layer == _playerRagdolLayerIndex)
                    {
                        isHitCanMadeImpact = true;
                        _bodyPart = "Head";
                    }
                    else
                    {
                        isHitCanMadeImpact = false;
                    }
                }
                else
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
                if (_bodyPart == "Body")
                {
                    _projectileSpawnTransform.LookAt(_playerView.PlayerTransform);
                    GameObject projectile = GameObject.Instantiate(_projectilePrefab,
                        _projectileSpawnTransform.position, _projectileSpawnTransform.rotation, _projectilesSpawnRoot.transform);
                    Projectile projectileView = projectile.GetOrAddComponent<Projectile>();
                    projectileView.StartMoving(_projectileSpawnTransform.position,
                        _playerView.PlayerDamagableZoneTransform.position,
                        gameObject.transform);
                }
                else
                {
                    _projectileSpawnTransform.LookAt(_playerView.PlayerTransform);
                    GameObject projectile = GameObject.Instantiate(_projectilePrefab,
                        _projectileSpawnTransform.position, _projectileSpawnTransform.rotation, _projectilesSpawnRoot.transform);
                    Projectile projectileView = projectile.GetOrAddComponent<Projectile>();
                    projectileView.StartMoving(_projectileSpawnTransform.position,
                        _playerView.PlayerHeadTransform.position,
                        gameObject.transform);
                }
            }

        }
    }
}
