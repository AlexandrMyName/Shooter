using System.Collections;
using System.Numerics;
using Extentions;
using UnityEditor;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class ShootingController : MonoBehaviour
{
    public KeyCode keyCode = KeyCode.Mouse0;
    public int Damage;
    public int Delay;
    public float ProjectileSpeed;
    public float RecoilHorizontal;
    public float RecoilVertical;
    public float RecoilModifierDelta;
    public Transform WeaponTransform;
    public GameObject PointOfHitObject;
    public GameObject PointOfWeaponHitObject;
    public GameObject MarkObject;
    public GameObject MarksRootObject;
    private bool _isShooting = false;
    private int _timeUntillShoot;
    private Transform _cameraTransform;
    private float _distance;
    private float _recoilModifier;


    private void Start()
    {
        _cameraTransform = Camera.main.transform;
        _timeUntillShoot = 0;
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
            _recoilModifier = 0;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
#if UNITY_EDITOR
            if(EditorApplication.isPlaying)
            {
                UnityEditor.EditorApplication.isPaused = true;
            }
#endif
        }
    }

    private void FixedUpdate()
    {
        GameObject hittedObject = GetShootHitObject();
        if (_isShooting && _timeUntillShoot <= 0)
        {
            Shoot(hittedObject);
            _timeUntillShoot = Delay;
        }
        else if (_timeUntillShoot > 0)
        {
            _timeUntillShoot--;
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
            WeaponTransform.LookAt(PointOfHitObject.transform);
        }
        else
        {
            Debug.DrawRay(_cameraTransform.position, _cameraTransform.TransformDirection(Vector3.forward) * 1000,
                Color.red);
            PointOfHitObject.SetActive(false);
        }
        
        Vector3 direction = GetRecoilVector();
        
        bool isWeaponRayHitTarget = Physics.Raycast(WeaponTransform.position,
            WeaponTransform.TransformDirection(direction), out weaponHit,
            Mathf.Infinity);
        GameObject hitedObject = null;
        
        if (isWeaponRayHitTarget)
        {
            Debug.DrawRay(WeaponTransform.position,
                WeaponTransform.TransformDirection(direction) * weaponHit.distance,
                Color.yellow);
            Debug.Log("Did Hit");
            if (_isShooting)
            {
                _distance = weaponHit.distance;
            }
            MoveHitMarkObject(PointOfWeaponHitObject, weaponHit);
            hitedObject = weaponHit.collider.gameObject;
        }
        else
        {
            Debug.DrawRay(WeaponTransform.position, WeaponTransform.TransformDirection(Vector3.forward) * 1000,
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
        if (_recoilModifier < 1)
        {
            _recoilModifier += RecoilModifierDelta;
        }

        float hitDelay = _distance / ProjectileSpeed;
        Transform hitTransform = PointOfWeaponHitObject.transform;
        StartCoroutine(Shooting(hitDelay, hitTransform, hitedObject));
    }

    private IEnumerator Shooting(float delay, Transform hitTransform, GameObject hitedObject)
    {
        yield return new WaitForSeconds(delay);
        if (hitedObject != null)
        {
            if (hitedObject.TryGetComponent<EnemyView>(out EnemyView enemyView))
            {
                enemyView.TakeDamage(Damage);
            }
            else
            {
                Vector3 hitPosition = hitTransform.position;
                GameObject hitMark = Instantiate(MarkObject, hitPosition, hitTransform.rotation,
                    MarksRootObject.transform);
            }
        }
        //Debug.Log($"{delay} Hit");
        yield return null;
    }

    private Vector3 GetRecoilVector()
    {
        Vector3 recoiledDirection = Vector3.forward;
        float x = Extention.GetRandomFloat(-RecoilHorizontal, RecoilHorizontal) * _recoilModifier;
        float y = Extention.GetRandomFloat(-RecoilVertical, RecoilVertical) * _recoilModifier;
        recoiledDirection = new Vector3(x, y, 1);
        return recoiledDirection;
    }
}
