using Configs;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private ProjectileConfig _projectileConfig;
    [SerializeField] private Rigidbody _projectileRigidbody;

    [SerializeField] private int _groundLayerIndex = 6;
    [SerializeField] private int _wallLayerIndex = 7;
    
    private Transform _hitEffectsRoot;
    private Vector3 _direction;
    private Vector3 _startPosition;
    private Vector3 _projectileLastPosition;
    private float _currentLifetime;
    
    void OnEnable()
    {
        _direction = Vector3.zero;
    }

    public void StartMoving(Vector3 startPosition, Vector3 hitPosition, Transform hitEffectsRoot)
    {
        _hitEffectsRoot = hitEffectsRoot;
        _startPosition = startPosition;
        _projectileLastPosition = startPosition;
        _direction = (hitPosition - startPosition).normalized;
        _currentLifetime = _projectileConfig.MaxLifetime;
    }

    void FixedUpdate()
    {
        _projectileRigidbody.velocity = _direction * _projectileConfig.ProjectileSpeed;
        _projectileLastPosition = gameObject.transform.position - _direction;
        _currentLifetime--;
        if (_currentLifetime <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        RaycastHit hitPoint;
        LayerMask layerMask;
        layerMask = 1 << _groundLayerIndex;
        layerMask |= 1 << _wallLayerIndex;
        bool isMadeImpact = _projectileConfig.IsMadeImpact;
        Collider[] hitColliders = Physics.OverlapSphere(gameObject.transform.position, _projectileConfig.DamageRadius);
        
        foreach (var hitCollider in hitColliders)
        {
            bool isEnemyHited = hitCollider.TryGetComponent<EnemyView>(out EnemyView enemyView);
            if (isEnemyHited)
            {
                enemyView.TakeDamage(_projectileConfig.Damage);
                isMadeImpact = false;
            }
        }
        
        bool isHitCanMadeImpact = Physics.Raycast(_projectileLastPosition,
            _direction, out hitPoint, Mathf.Infinity, layerMask);
        if (isMadeImpact && isHitCanMadeImpact)
        {
            Instantiate(_projectileConfig.ImpactParticleSystem, hitPoint.point, Quaternion.LookRotation(hitPoint.normal),
                _hitEffectsRoot);
        }
        Destroy(gameObject);
    }
    
}
