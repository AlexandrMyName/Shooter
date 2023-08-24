using System.Collections;
using Extentions;
using UnityEditor;
using UnityEngine;

public class ShootingController : MonoBehaviour
{
    [SerializeField] private KeyCode _shootKeyCode = KeyCode.Mouse0;
    [SerializeField] private int _damage;
    [SerializeField] float _maxDistance;
    [SerializeField] private float _shootDelay;
    [SerializeField] private float _projectileSpeed;
    [SerializeField] private float _spreadingHorizontal;
    [SerializeField] private float _spreadingVertical;
    [SerializeField] private float _spreadingModifierDelta;
    [SerializeField] private float _spreadingDefaultModifier;
    [SerializeField] private float _recoilHorizontal;
    [SerializeField] private float _recoilVertical;
    [SerializeField] private float _recoilModifierDeltaHorizontal;
    [SerializeField] private float _recoilModifierDeltaVertical;
    [SerializeField] private Transform _projectileSpawnTransform;
    [SerializeField] private Transform _hitEffectsRoot;
    [SerializeField] private GameObject _pointOfHitObject;
    [SerializeField] private RectTransform _crosshairTransform;
    [SerializeField] private TrailRenderer _bulletTrail;
    [SerializeField] private ParticleSystem _impactParticleSystem;
    [SerializeField] private ParticleSystem _shootingParticleSystem;
    [SerializeField] private Animator _animator;
    
    private bool _isShooting = false;
    private float _lastShootTime;
    private Transform _cameraTransform;
    private float _spreadingModifier;
    private float _recoilModifierHorizontal;
    private float _recoilModifierVertical;
    private RaycastHit _hitTarget;


    private void Start()
    {
        _cameraTransform = Camera.main.transform;
        _pointOfHitObject.SetActive(false);
        if (_projectileSpeed <= 0)
        {
            _projectileSpeed = 0.1f;
            Debug.Log("Wrong projectile speed");
        }

        _spreadingModifier = _spreadingDefaultModifier;
    }

    private void Update () 
    {
        if (Input.GetKey(_shootKeyCode))
        {
            _isShooting = true;
        }
        else
        {
            _isShooting = false;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
#if UNITY_EDITOR
            if(EditorApplication.isPlaying)
            {
                UnityEditor.EditorApplication.isPaused = true;
            }
#else
            Application.Quit();
#endif
        }
    }

    private void FixedUpdate()
    {
        GameObject hittedObject = GetShootHitObject();

        if (_isShooting && (_lastShootTime + _shootDelay < Time.time))
        {
            _shootingParticleSystem.Play();
            Shoot(hittedObject);
            _lastShootTime = Time.time;
        }
        
        if (!_isShooting)
        {
            ChangeSpread(-0.1f);
            ChangeCrosshairScale();
            ChangeRecoilVector(-_recoilModifierDeltaHorizontal, -_recoilModifierDeltaVertical);
        }
    }

    private GameObject GetShootHitObject()
    {
        RaycastHit cameraHit;
        RaycastHit weaponHit;
        bool isCameraRayHitTarget = Physics.Raycast(_cameraTransform.position,
            _cameraTransform.TransformDirection(Vector3.forward), out cameraHit,
            Mathf.Infinity);

        if (isCameraRayHitTarget)
        {
            DebugHitInfo(true, _cameraTransform.position,
                _cameraTransform.TransformDirection(Vector3.forward) * cameraHit.distance);
            MoveHitMarkObject(_pointOfHitObject, cameraHit.point, cameraHit.normal);
            _projectileSpawnTransform.LookAt(_pointOfHitObject.transform);
        }
        else
        {
            DebugHitInfo(false, _cameraTransform.position,
                _cameraTransform.TransformDirection(Vector3.forward) * 1000);
            Vector3 dir = _cameraTransform.position + (_cameraTransform.TransformDirection(Vector3.forward) * 1000);
            MoveHitMarkObject(_pointOfHitObject, dir, Vector3.forward);
            _projectileSpawnTransform.LookAt(_pointOfHitObject.transform);
        }
        
        Vector3 direction = GetRecoilVector();
        
        bool isWeaponRayHitTarget = Physics.Raycast(_projectileSpawnTransform.position,
            _projectileSpawnTransform.TransformDirection(direction), out weaponHit,
            Mathf.Infinity);
        GameObject hitedObject = null;
        
        if (isWeaponRayHitTarget)
        {
            DebugHitInfo(true, _projectileSpawnTransform.position,
                _projectileSpawnTransform.TransformDirection(direction) * weaponHit.distance);
            hitedObject = weaponHit.collider.gameObject;
            _hitTarget = weaponHit;
        }
        else
        {
            DebugHitInfo(false, _projectileSpawnTransform.position,
                _projectileSpawnTransform.TransformDirection(Vector3.forward) * 1000);
        }

        return hitedObject;
    }

    private void MoveHitMarkObject(GameObject markObject, Vector3 rayHitPoint, Vector3 rayHitNormal)
    {
        markObject.transform.position = rayHitPoint;
        markObject.transform.rotation = Quaternion.LookRotation(rayHitNormal);
        markObject.SetActive(true);
    }

    private void DebugHitInfo(bool isHit, Vector3 startPos, Vector3 endPos)
    {

        if (isHit)
        {
            Debug.DrawRay(startPos, endPos, Color.yellow);
        }
        else
        {
            Debug.DrawRay(startPos, endPos, Color.red);
        }
        
    }

    private void Shoot(GameObject hitedObject)
    {
        //Animator.SetBool("IsShooting", true);
        if (_recoilModifierHorizontal >= _recoilHorizontal && _recoilModifierVertical >= _recoilVertical)
        {
            ChangeSpread(_spreadingModifierDelta);
        }
        ChangeCrosshairScale();
        ChangeRecoilVector(_recoilModifierDeltaHorizontal, _recoilModifierDeltaVertical);
        
        
        TrailRenderer trail = Instantiate(_bulletTrail, _projectileSpawnTransform.position, Quaternion.identity,
            _hitEffectsRoot);
        if (hitedObject != null)
        {
            StartCoroutine(SpawnTrail(trail, _hitTarget.point, _hitTarget.normal, true));
        }
        else
        {
            StartCoroutine(SpawnTrail(trail, 
                _projectileSpawnTransform.TransformDirection(GetRecoilVector()) * 100,
                Vector3.zero, true));
        }
    }

    private Vector3 GetRecoilVector()
    {
        Vector3 recoiledDirection = Vector3.forward;

        float x = _recoilModifierHorizontal;
        float y = _recoilModifierVertical;
        
        float spreadX = Extention.GetRandomFloat(-_spreadingHorizontal, _spreadingHorizontal) * _spreadingModifier;
        float spreadY = Extention.GetRandomFloat(-_spreadingVertical, _spreadingVertical) * _spreadingModifier;
        recoiledDirection = new Vector3(x + spreadX, y + spreadY, 1);
        return recoiledDirection;
    }
    
    private IEnumerator SpawnTrail(TrailRenderer trail, Vector3 hitPoint, Vector3 hitNormal,
        bool isMadeImpact)
    {
        GameObject hitedObject;
        float currentMaxDistance = _maxDistance;
        Vector3 startPosition = trail.transform.position;
        float distance = Vector3.Distance(trail.transform.position, hitPoint);
        float remainingDistance = distance;

        while (remainingDistance > 0)
        {
            trail.transform.position = Vector3.Lerp(startPosition, hitPoint, 1 - (remainingDistance / distance));
            remainingDistance -= _projectileSpeed * Time.deltaTime;
            currentMaxDistance -= _projectileSpeed * Time.deltaTime;
            if (currentMaxDistance <= 0)
            {
                break;
            }
            RaycastHit weaponHit;
            bool isWeaponRayHitTarget = Physics.Raycast(trail.transform.position,
                trail.transform.TransformDirection(-hitNormal), out weaponHit,
                0.1f);
            if (isWeaponRayHitTarget)
            {
                hitedObject = weaponHit.collider.gameObject;
                if (hitedObject != null)
                {
                    if (hitedObject.TryGetComponent<EnemyView>(out EnemyView enemyView))
                    {
                        enemyView.TakeDamage(_damage);
                    }
                    else if (isMadeImpact)
                    {
                        Instantiate(_impactParticleSystem, hitPoint, Quaternion.LookRotation(hitNormal),
                            _hitEffectsRoot);
                    }
                }
                break;
            }
            if (remainingDistance < 0)
            {
                if (currentMaxDistance - distance <= 0)
                {
                    break;
                }
                bool isParticleRayHitTarget = Physics.Raycast(hitPoint,
                    hitPoint - startPosition, out weaponHit,
                    currentMaxDistance);
                if (isParticleRayHitTarget)
                {
                    Debug.Log(currentMaxDistance);
                    startPosition = hitPoint;
                    hitPoint = weaponHit.point;
                    hitNormal = weaponHit.normal;
                    distance = Vector3.Distance(startPosition, hitPoint);
                    remainingDistance = distance;
                }
            }
            yield return null;
        }
        Destroy(trail.gameObject, trail.time);
        //Debug.Log("EndShoot");
        //Animator.SetBool("IsShooting", false);
    }


    private void ChangeRecoilVector(float recoilHorizontalDelta,float recoilVerticalDelta)
    {
        bool isRecoilHorizontalIncreasing = _recoilModifierHorizontal < _recoilHorizontal && recoilHorizontalDelta > 0;
        bool isRecoilHorizontalDecreasing = _recoilModifierHorizontal > 0 && recoilHorizontalDelta < 0;
        if (isRecoilHorizontalIncreasing || isRecoilHorizontalDecreasing)
        {
            _recoilModifierHorizontal += recoilHorizontalDelta;
        }

        bool isRecoilVerticalIncreasing = _recoilModifierVertical < _recoilVertical && recoilVerticalDelta > 0;
        bool isRecoilVerticalDecreasing = _recoilModifierVertical > 0 && recoilVerticalDelta < 0;
        if (isRecoilVerticalIncreasing || isRecoilVerticalDecreasing)
        {
            _recoilModifierVertical += recoilVerticalDelta;
        }
    }

    private void ChangeSpread(float spreadDelta)
    {
        bool isSpreadIncreasing = spreadDelta > 0 && _spreadingModifier < 1;
        bool isSpreadDecreasing = spreadDelta < 0 && _spreadingModifier > _spreadingDefaultModifier;
        if (isSpreadIncreasing || isSpreadDecreasing)
        {
            _spreadingModifier += spreadDelta;
        }
    }
    
    private void ChangeCrosshairScale()
    {
        if (_spreadingModifier >= 0 && _spreadingModifier <= 1)
        {
            Vector3 corsshairRecoiledScale = _crosshairTransform.localScale;
            corsshairRecoiledScale = new Vector3(1 + _spreadingModifier,
                1 + _spreadingModifier, corsshairRecoiledScale.z);
            _crosshairTransform.localScale = corsshairRecoiledScale;
        }
    }
}
