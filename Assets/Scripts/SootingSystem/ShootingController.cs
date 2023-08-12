using System.Collections;
using Extentions;
using UnityEditor;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class ShootingController : MonoBehaviour
{
    public KeyCode keyCode = KeyCode.Mouse0;
    public int Damage;
    public float ShootDelay;
    public float ProjectileSpeed;
    public float RecoilHorizontal;
    public float RecoilVertical;
    public float RecoilModifierDelta;
    public Transform ProjectileSpawnTransform;
    public GameObject PointOfHitObject;
    public GameObject PointOfWeaponHitObject;
    public RectTransform CrosshairTransform;
    [SerializeField]
    private TrailRenderer _bulletTrail;
    [SerializeField]
    private ParticleSystem _impactParticleSystem;
    [SerializeField]
    private ParticleSystem _shootingParticleSystem;
    [SerializeField]
    private Animator Animator;
    
    private bool _isShooting = false;
    private float _lastShootTime;
    private Transform _cameraTransform;
    private float _recoilModifier;
    private RaycastHit _hitTarget;


    private void Start()
    {
        _cameraTransform = Camera.main.transform;
        PointOfHitObject.SetActive(false);
        if (ProjectileSpeed <= 0)
        {
            ProjectileSpeed = 0.1f;
            Debug.Log("Wrong projectile speed");
        }
    }

    private void Update () 
    {
        if (Input.GetKey(keyCode))
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

        if (_isShooting && (_lastShootTime + ShootDelay < Time.time))
        {
            _shootingParticleSystem.Play();
            Shoot(hittedObject);
            _lastShootTime = Time.time;
        }
        
        if (!_isShooting && _recoilModifier > 0)
        {
            _recoilModifier -= 0.1f;
            Vector3 corsshairRecoiledScale = CrosshairTransform.localScale;
            corsshairRecoiledScale = new Vector3(1 + _recoilModifier,
                1 + _recoilModifier, corsshairRecoiledScale.z);
            CrosshairTransform.localScale = corsshairRecoiledScale;
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
            Debug.DrawRay(_cameraTransform.position,
                _cameraTransform.TransformDirection(Vector3.forward) * cameraHit.distance,
                Color.yellow);
            MoveHitMarkObject(PointOfHitObject, cameraHit);
            ProjectileSpawnTransform.LookAt(PointOfHitObject.transform);
        }
        else
        {
            Debug.DrawRay(_cameraTransform.position, _cameraTransform.TransformDirection(Vector3.forward) * 1000,
                Color.red);
            PointOfHitObject.SetActive(false);
        }
        
        Vector3 direction = GetRecoilVector();
        
        bool isWeaponRayHitTarget = Physics.Raycast(ProjectileSpawnTransform.position,
            ProjectileSpawnTransform.TransformDirection(direction), out weaponHit,
            Mathf.Infinity);
        GameObject hitedObject = null;
        
        if (isWeaponRayHitTarget)
        {
            Debug.DrawRay(ProjectileSpawnTransform.position,
                ProjectileSpawnTransform.TransformDirection(direction) * weaponHit.distance,
                Color.yellow);
            Debug.Log("Did Hit");
            MoveHitMarkObject(PointOfWeaponHitObject, weaponHit);
            hitedObject = weaponHit.collider.gameObject;
            _hitTarget = weaponHit;
        }
        else
        {
            Debug.DrawRay(ProjectileSpawnTransform.position, ProjectileSpawnTransform.TransformDirection(Vector3.forward) * 1000,
                Color.red);
            Debug.Log("Did not Hit");
            PointOfWeaponHitObject.SetActive(false);
        }

        return hitedObject;
    }

    private void MoveHitMarkObject(GameObject markObject, RaycastHit rayHit)
    {
        markObject.transform.position = new Vector3(rayHit.point.x, rayHit.point.y, rayHit.point.z);
        markObject.transform.rotation = Quaternion.LookRotation(rayHit.normal);
        markObject.SetActive(true);
    }

    private void Shoot(GameObject hitedObject)
    {
        //Animator.SetBool("IsShooting", true);
        if (_recoilModifier < 1)
        {
            _recoilModifier += RecoilModifierDelta;
            Vector3 corsshairRecoiledScale = CrosshairTransform.localScale;
            corsshairRecoiledScale = new Vector3(1 + _recoilModifier,
                1 + _recoilModifier, corsshairRecoiledScale.z);
            CrosshairTransform.localScale = corsshairRecoiledScale;
        }
        TrailRenderer trail = Instantiate(_bulletTrail, ProjectileSpawnTransform.position, Quaternion.identity);
        if (hitedObject != null)
        {
            StartCoroutine(SpawnTrail(trail, _hitTarget.point, _hitTarget.normal, true, hitedObject));
        }
        else
        {
            StartCoroutine(SpawnTrail(trail, ProjectileSpawnTransform.position + GetRecoilVector() * 100,
                Vector3.zero, false, hitedObject));
        }
    }

    private Vector3 GetRecoilVector()
    {
        Vector3 recoiledDirection = Vector3.forward;
        float x = Extention.GetRandomFloat(-RecoilHorizontal, RecoilHorizontal) * _recoilModifier;
        float y = Extention.GetRandomFloat(-RecoilVertical, RecoilVertical) * _recoilModifier;
        recoiledDirection = new Vector3(x, y, 1);
        return recoiledDirection;
    }
    
    private IEnumerator SpawnTrail(TrailRenderer Trail, Vector3 HitPoint, Vector3 HitNormal,
        bool MadeImpact, GameObject hitedObject)
    {
        Vector3 startPosition = Trail.transform.position;
        float distance = Vector3.Distance(Trail.transform.position, HitPoint);
        float remainingDistance = distance;

        while (remainingDistance > 0)
        {
            Trail.transform.position = Vector3.Lerp(startPosition, HitPoint, 1 - (remainingDistance / distance));

            remainingDistance -= ProjectileSpeed * Time.deltaTime;

            yield return null;
        }
        //Animator.SetBool("IsShooting", false);
        Trail.transform.position = HitPoint;
        if (hitedObject != null)
        {
            if (hitedObject.TryGetComponent<EnemyView>(out EnemyView enemyView))
            {
                enemyView.TakeDamage(Damage);
            }
            else if (MadeImpact)
            {
                Instantiate(_impactParticleSystem, HitPoint, Quaternion.LookRotation(HitNormal));
            }
        }
        Destroy(Trail.gameObject, Trail.time);
    }
}
