using System.Collections.Generic;
using Configs;
using EnemySystem;
using Player;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using System;


public class Projectile : MonoBehaviour, IDisposable
{

    [SerializeField] private ProjectileConfig _projectileConfig;
    [SerializeField] private Rigidbody _projectileRigidbody;

    [SerializeField] private int _groundLayerIndex = 6;
    [SerializeField] private int _wallLayerIndex = 7;
    [SerializeField] private LayerMask _explosionIncludeLayers;

    private List<IDisposable> _disposables = new List<IDisposable>();
    private Transform _hitEffectsRoot;
    private Vector3 _direction;
    private Vector3 _startPosition;
    private Vector3 _projectileLastPosition;
    private float _currentLifetime;
    private List<int> _currentIDs;
    private bool _palyerHitedOnce;


    void OnEnable() => _direction = Vector3.zero;
     

    public void Dispose() => _disposables.ForEach(disposable => disposable.Dispose());


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
        InitObservables();

    }


    private void InitObservables()
    {

        var projectTileCollider = GetComponent<Collider>();

        if (_projectileConfig.ProjectileType == ProjectileType.Rocket)
        {

            projectTileCollider.isTrigger = true;

            GetComponent<Collider>()
                .OnTriggerEnterAsObservable()
                .Subscribe(col =>
                {
                    _projectileRigidbody.constraints = RigidbodyConstraints.FreezeAll;
                    TakeExposionForce();
                })
                .AddTo(_disposables);

            GetComponent<Collider>()
               .OnCollisionEnterAsObservable()
               .Subscribe(col =>
               {
                   _projectileRigidbody.constraints = RigidbodyConstraints.FreezeAll;
                   TakeExposionForce();
               })
               .AddTo(_disposables);


        }
        else
        {
            projectTileCollider
               .OnCollisionEnterAsObservable()
               .Subscribe(col => _projectileRigidbody.constraints = RigidbodyConstraints.FreezeAll)
               .AddTo(_disposables);
        }
    }


    void FixedUpdate()
    {
        if (!_projectileConfig.IsGrenade)
        {
            _projectileRigidbody.velocity = _direction * _projectileConfig.ProjectileSpeed;
            _projectileLastPosition = gameObject.transform.position - _direction;
        }
        _currentLifetime--;
        if (_currentLifetime <= 0)
        {
            Destroy(gameObject);
        }
    }


    private void Update()
    {
        if (_projectileRigidbody.constraints == RigidbodyConstraints.FreezeAll)
        {
            RaycastHit hitPoint;
            LayerMask layerMask;

            layerMask = 1 << _groundLayerIndex;
            layerMask |= 1 << _wallLayerIndex;
            

            bool isMadeImpact = _projectileConfig.IsMadeImpact;
            Collider[] hitColliders = Physics.OverlapSphere(gameObject.transform.position, _projectileConfig.DamageRadius);
            _currentIDs = new List<int>();
            
            foreach (var hitCollider in hitColliders)
            {
                bool isEnemyHited = hitCollider.TryGetComponent<EnemyBoneView>(out EnemyBoneView enemyBoneView);
                bool isPlayerHited = hitCollider.TryGetComponent<PlayerBoneView>(out PlayerBoneView playerBoneView);

                if (isEnemyHited)
                {
                    foreach (var id in _currentIDs)
                    {
                        if (id == enemyBoneView.EnemyView.EnemyID)
                        {
                            isEnemyHited = false;
                        }
                    }
                }
                if (isEnemyHited)
                {
                    _currentIDs.Add(enemyBoneView.EnemyView.EnemyID);
                    enemyBoneView.EnemyView.TakeDamage(_projectileConfig.Damage);

                    isMadeImpact = false;
                }

                if (isPlayerHited && !_palyerHitedOnce) 
                {

                    if (_projectileConfig.DamageType == DamageType.WithOutPlayer) return;

                    playerBoneView.PlayerView.TakeDamage(_projectileConfig.Damage);
                    _palyerHitedOnce = true;
                }
            }
            
                _palyerHitedOnce = false;
                    
                bool isHitCanMadeImpact = Physics.Raycast(_projectileLastPosition,
                    _direction, out hitPoint, Mathf.Infinity, layerMask);
            
            if (isMadeImpact && isHitCanMadeImpact)
            {
                Instantiate(_projectileConfig.ImpactParticleSystem, hitPoint.point, 
                    Quaternion.LookRotation(hitPoint.normal), _hitEffectsRoot);
                
            }
            Destroy(gameObject);
        }
    }


    private void TakeExposionForce()
    {

        Collider[] hitColliders = Physics.OverlapSphere(gameObject.transform.position, _projectileConfig.DamageRadius, _explosionIncludeLayers);
  
        foreach (var coll in hitColliders)
        {

            var rb = coll.gameObject.GetComponent<Rigidbody>();
            
            if (rb != null) 
                rb.AddExplosionForce(
                    _projectileConfig.Damage,
                    transform.position,
                    _projectileConfig.DamageRadius,
                    0f,
                    ForceMode.Impulse);
        }
    }
     

    private void OnDestroy() => Dispose();

}
