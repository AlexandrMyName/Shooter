using System.Collections;
using Extentions;
using UnityEditor;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class ShootingController : MonoBehaviour
{
    [SerializeField] private KeyCode _shootKeyCode = KeyCode.Mouse0;
    [SerializeField] private int _damage;
    [SerializeField] private float _shootDelay;
    [SerializeField] private float _projectileSpeed;
    [SerializeField] private float _recoilHorizontal;
    [SerializeField] private float _recoilVertical;
    [SerializeField] private float _recoilModifierDelta;
    [SerializeField] private Transform _projectileSpawnTransform;
    [SerializeField] private GameObject _pointOfHitObject;
    [SerializeField] private GameObject _pointOfWeaponHitObject;
    [SerializeField] private RectTransform _crosshairTransform;
    [SerializeField] private TrailRenderer _bulletTrail;
    [SerializeField] private ParticleSystem _impactParticleSystem;
    [SerializeField] private ParticleSystem _shootingParticleSystem;
    [SerializeField] private Animator _animator;
    
    private bool _isShooting = false;
    private float _lastShootTime;
    private Transform _cameraTransform;
    private float _recoilModifier;
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
        
        if (!_isShooting && _recoilModifier > 0)
        {
            ChangeCrosshairScale(-0.1f);
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
            //MoveHitMarkObject(_pointOfWeaponHitObject, weaponHit.point, weaponHit.normal);
            hitedObject = weaponHit.collider.gameObject;
            _hitTarget = weaponHit;
        }
        else
        {
            DebugHitInfo(false, _projectileSpawnTransform.position,
                _projectileSpawnTransform.TransformDirection(Vector3.forward) * 1000);
            _pointOfWeaponHitObject.SetActive(false);
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
        if (_recoilModifier < 1)
        {
            ChangeCrosshairScale(_recoilModifierDelta);
        }
        TrailRenderer trail = Instantiate(_bulletTrail, _projectileSpawnTransform.position, Quaternion.identity);
        if (hitedObject != null)
        {
            StartCoroutine(SpawnTrail(trail, _hitTarget.point, _hitTarget.normal, true, hitedObject));
        }
        else
        {
            StartCoroutine(SpawnTrail(trail, 
                _projectileSpawnTransform.TransformDirection(GetRecoilVector()) * 100,
                Vector3.zero, false, hitedObject));
        }
    }

    private Vector3 GetRecoilVector()
    {
        Vector3 recoiledDirection = Vector3.forward;
        float x = Extention.GetRandomFloat(-_recoilHorizontal, _recoilHorizontal) * _recoilModifier;
        float y = Extention.GetRandomFloat(-_recoilVertical, _recoilVertical) * _recoilModifier;
        recoiledDirection = new Vector3(x, y, 1);
        return recoiledDirection;
    }
    
    private IEnumerator SpawnTrail(TrailRenderer trail, Vector3 hitPoint, Vector3 hitNormal,
        bool isMadeImpact, GameObject hitedObject)
    {
        Vector3 startPosition = trail.transform.position;
        float distance = Vector3.Distance(trail.transform.position, hitPoint);
        if (!isMadeImpact)
        {
            distance = 20f;
        }
        float remainingDistance = distance;

        while (remainingDistance > 0)
        {
            trail.transform.position = Vector3.Lerp(startPosition, hitPoint, 1 - (remainingDistance / distance));

            remainingDistance -= _projectileSpeed * Time.deltaTime;

            yield return null;
        }
        //Animator.SetBool("IsShooting", false);
        trail.transform.position = hitPoint;
        if (hitedObject != null)
        {
            if (hitedObject.TryGetComponent<EnemyView>(out EnemyView enemyView))
            {
                enemyView.TakeDamage(_damage);
            }
            else if (isMadeImpact)
            {
                Instantiate(_impactParticleSystem, hitPoint, Quaternion.LookRotation(hitNormal));
            }
        }
        Destroy(trail.gameObject, trail.time);
    }

    private void ChangeCrosshairScale(float scaleDelta)
    {
        _recoilModifier += scaleDelta;
        Vector3 corsshairRecoiledScale = _crosshairTransform.localScale;
        corsshairRecoiledScale = new Vector3(1 + _recoilModifier,
            1 + _recoilModifier, corsshairRecoiledScale.z);
        _crosshairTransform.localScale = corsshairRecoiledScale;
    }
}
