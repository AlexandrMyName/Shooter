using System.Collections.Generic;
using Configs;
using EnemySystem;
using Player;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using System;
using System.Linq;
using UnityEngine.Serialization;


namespace ShootingSystem
{

    public class Projectile : MonoBehaviour, IDisposable
    {

        [SerializeField] private ProjectileConfig _projectileConfig;
        [SerializeField] private Rigidbody _projectileRigidbody;
        [SerializeField] private SphereCollider _collider;

        [SerializeField] private LayerMask _hitImpactLayers;
        [SerializeField] private LayerMask _enemyRagdollLayer;
        [SerializeField] private LayerMask _playerRagdollLayer;
        [SerializeField] private LayerMask _explosionIncludeLayers;

        private RaycastHit _hitPoint;
        private Collider[] _playerHitColliders;
        private Collider[] _enemyHitColliders;
        private Collider[] _defaultHitColliders;

        private List<IDisposable> _disposables = new();

        private Transform _hitEffectsRoot;
        private Vector3 _direction;
        private Vector3 _startPosition;
        private Vector3 _projectileLastPosition;

        private float _currentLifetime;

        private bool _playerHitOnce;
        private bool _isMadeImpact;
        private bool _isProjectileCollided;
        private bool _isEnemyHit;
        private bool _isPlayerHit;


        private void OnEnable() => _direction = Vector3.zero;


        private void Start()
        {
            _isMadeImpact = _projectileConfig.IsMadeImpact;
            InitObservables();
        }


        private void Update()
        {
            ControlProjectileLifeTime();
        }


        private void FixedUpdate()
        {
            if (!_projectileConfig.IsGrenade)
            {
                _projectileRigidbody.velocity = _direction * _projectileConfig.ProjectileSpeed;
                _projectileLastPosition = gameObject.transform.position - _direction;
            }

            if (_isProjectileCollided)
            {
                _isProjectileCollided = false;
                ProcessHitLogic();
            }

        }

        public void SetColliderRadius(float radius)
        {
            _collider.radius = radius;
        }
        public void FreezeRigidbody()
        {
            _projectileRigidbody.constraints = RigidbodyConstraints.FreezeAll;
            _isProjectileCollided = true;
        }

        private void InitObservables()
        {
            var projectTileCollider = GetComponent<Collider>();

            projectTileCollider
                .OnCollisionEnterAsObservable()
                .Subscribe(col =>
                {
                    FreezeRigidbody();
                })
                .AddTo(_disposables);
        }

        private void ProcessHitLogic()
        {
            _playerHitColliders = Physics.OverlapSphere(
                gameObject.transform.position,
                _projectileConfig.DamageRadius,
                _playerRagdollLayer);

            _enemyHitColliders = Physics.OverlapSphere(
                gameObject.transform.position,
                _projectileConfig.DamageRadius,
                _enemyRagdollLayer);

            if (!_projectileConfig.ProjectileType.Equals(ProjectileType.Rocket))
            {
                if (_playerHitColliders.Length > 0)
                {
                    ProcessPlayerHit(_playerHitColliders);
                    _isMadeImpact = false;
                }

                if (_enemyHitColliders.Length > 0)
                {
                    ProcessEnemyHit(_enemyHitColliders);
                    _isMadeImpact = false;
                }
            }

            if (_projectileConfig.ProjectileType.Equals(ProjectileType.Rocket))
            {
                _defaultHitColliders = Physics.OverlapSphere(
                    gameObject.transform.position,
                    _projectileConfig.DamageRadius,
                    _explosionIncludeLayers);

                ProcessExplosionHits(_playerHitColliders, _enemyHitColliders, _defaultHitColliders);
            }

            ProcessImpactOnTheWallsAndGround();

            Destroy(gameObject);
        }


        private void ProcessExplosionHits(Collider[] playerHits, Collider[] enemyHits, Collider[] defaultHits)
        {
            for (int i = 0; i < defaultHits.Length; i++)
            {
                if (defaultHits[i] != null && defaultHits[i].TryGetComponent<Rigidbody>(out var rb))
                {
                    rb.AddExplosionForce(
                        _projectileConfig.Force * rb.mass,
                        transform.position,
                        _projectileConfig.DamageRadius,
                        0.5f,
                        ForceMode.Impulse);
                }
            }

            Dictionary<EnemyView, List<Rigidbody>> enemiesRbsByView = enemyHits
                .Select(collider => collider.attachedRigidbody)
                .Where(rb => rb != null)
                .GroupBy(rb => rb.transform.GetComponent<EnemyBoneView>().EnemyView)
                .ToDictionary(group => group.Key, group => group.ToList());

            foreach (var enemyRbsGroup in enemiesRbsByView)
            {
                enemyRbsGroup.Key.ProcessExplosionDamageForRbs(
                    enemyRbsGroup.Value, _projectileConfig.DamageRadius, _projectileConfig.Force,
                    _projectileConfig.Damage, _projectileConfig.UpwardsModifier, _direction,
                    transform.position);
            }

            if (_projectileConfig.DamageType == DamageType.PlayerIcluded && playerHits != null && playerHits.Length > 0)
            {
                var playerBoneView = playerHits[0].GetComponentInParent<PlayerBoneView>();
                if (playerBoneView != null)
                {
                    playerBoneView.PlayerView.TakeDamage(_projectileConfig.Damage);
                    playerBoneView.PlayerView.RagdollStun();
                    Observable.Timer(TimeSpan.FromSeconds(0.5)).Subscribe(_ =>
                    {
                        playerBoneView.PlayerView.RagdollUnStun();
                    });
                }
            }
        }


        private void ProcessImpactOnTheWallsAndGround()
        {
            bool isHitCanMadeImpact = Physics.Raycast(_projectileLastPosition,
                _direction, out var hitPoint, Mathf.Infinity, _hitImpactLayers);

            if (_isMadeImpact && isHitCanMadeImpact)
            {
                Instantiate(_projectileConfig.ImpactParticleSystem, hitPoint.point,
                    Quaternion.LookRotation(hitPoint.normal), _hitEffectsRoot);
            }
        }


        private void ProcessEnemyHit(Collider[] enemyHitColliders)
        {
            if (enemyHitColliders[0].TryGetComponent(out EnemyBoneView enemyBoneView))
            {
                enemyBoneView.EnemyView.TakeForceDamage(
                    enemyBoneView.GetComponent<Rigidbody>(),
                    _projectileConfig.Damage,
                    _projectileConfig.ProjectileSpeed,
                    _direction);
            }
            else
            {
                // This is necessary as we have collider without rigid body on feet of ragdoll
                var enemyBoneViewParent = enemyHitColliders[0].GetComponentInParent<EnemyBoneView>();
                if (enemyBoneViewParent)
                    enemyBoneViewParent.EnemyView.TakeForceDamage(
                        enemyBoneView.GetComponent<Rigidbody>(),
                        _projectileConfig.Damage,
                        _projectileConfig.ProjectileSpeed,
                        _direction);
                else
                    throw new ArgumentException("Expected PlayerBoneView - but found nothing");
            }
        }


        private void ProcessPlayerHit(Collider[] playerHitColliders)
        {
            if (playerHitColliders[0].TryGetComponent(out PlayerBoneView playerBoneView))
            {
                playerBoneView.PlayerView.TakeDamage(_projectileConfig.Damage);
            }
            else
            {
                // This is necessary as we have collider without rigid body on feet of ragdoll
                var playerBoneViewParent = playerHitColliders[0].GetComponentInParent<PlayerBoneView>();
                if (playerBoneViewParent)
                    playerBoneViewParent.PlayerView.TakeDamage(_projectileConfig.Damage);
                else
                    throw new ArgumentException("Expected PlayerBoneView - but found nothing");
            }
        }


        private void ControlProjectileLifeTime()
        {
            _currentLifetime--;
            if (_currentLifetime <= 0)
            {
                Destroy(gameObject);
            }
        }


        public void StartMoving(Vector3 startPosition, Vector3 hitPosition, Transform hitEffectsRoot)
        {
            _hitEffectsRoot = hitEffectsRoot;
            _startPosition = startPosition;
            _projectileLastPosition = startPosition;
            _direction = (hitPosition - startPosition).normalized;
            _currentLifetime = _projectileConfig.MaxLifetime;

            if (_projectileConfig.IsGrenade)
            {
                _projectileRigidbody.AddForce(_direction * _projectileConfig.ProjectileSpeed);
            }
        }


        private void OnDestroy() => Dispose();


        public void Dispose() => _disposables.ForEach(disposable => disposable.Dispose());


    }
}