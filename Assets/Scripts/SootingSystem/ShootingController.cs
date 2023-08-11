using UnityEditor;
using UnityEngine;

public class ShootingController : MonoBehaviour
{
    public KeyCode keyCode = KeyCode.Mouse0;
    public int Delay;
    public Transform WeaponTransform;
    public GameObject PointOfHitObject;
    public GameObject PointOfWeaponHitObject;
    public GameObject MarkObject;
    public GameObject MarksRootObject;
    private bool _isShooting = false;
    private int _timeUntillShoot;
    private Transform cameraTransform;


    private void Start()
    {
        cameraTransform = Camera.main.transform;
        _timeUntillShoot = 0;
        PointOfHitObject.SetActive(false);
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
#endif
        }
    }

    private void FixedUpdate()
    {
        CalculateShootHit();
        if (_isShooting && _timeUntillShoot <= 0)
        {
            Shoot();
            _timeUntillShoot = Delay;
        }
        else if (_timeUntillShoot > 0)
        {
            _timeUntillShoot--;
        }
    }

    private void CalculateShootHit()
    {
        RaycastHit cameraHit;
        RaycastHit weaponHit;
        bool isCameraRayHitTarget = Physics.Raycast(cameraTransform.position,
            cameraTransform.TransformDirection(Vector3.forward), out cameraHit,
            Mathf.Infinity);

        if (isCameraRayHitTarget)
        {
            Debug.DrawRay(cameraTransform.position, cameraTransform.TransformDirection(Vector3.forward) * cameraHit.distance,
                Color.yellow);
            PointOfHitObject.transform.position = new Vector3(cameraHit.point.x, cameraHit.point.y, cameraHit.point.z);
            PointOfHitObject.transform.rotation = Quaternion.LookRotation(cameraHit.normal);
            PointOfHitObject.SetActive(true);
            WeaponTransform.LookAt(PointOfHitObject.transform);
        }
        else
        {
            Debug.DrawRay(cameraTransform.position, cameraTransform.TransformDirection(Vector3.forward) * 1000,
                Color.red);
            PointOfHitObject.SetActive(false);
        }
        
        bool isWeaponRayHitTarget = Physics.Raycast(WeaponTransform.position,
            WeaponTransform.TransformDirection(Vector3.forward), out weaponHit,
            Mathf.Infinity);
        
        if (isWeaponRayHitTarget)
        {
            Debug.DrawRay(WeaponTransform.position,
                WeaponTransform.TransformDirection(Vector3.forward) * weaponHit.distance,
                Color.yellow);
            Debug.Log("Did Hit");
            
            PointOfWeaponHitObject.transform.position = new Vector3(weaponHit.point.x, weaponHit.point.y, weaponHit.point.z);
            PointOfWeaponHitObject.transform.rotation = Quaternion.LookRotation(weaponHit.normal);
            PointOfWeaponHitObject.SetActive(true);
        }
        else
        {
            Debug.DrawRay(WeaponTransform.position, WeaponTransform.TransformDirection(Vector3.forward) * 1000,
                Color.red);
            Debug.Log("Did not Hit");
            PointOfWeaponHitObject.SetActive(false);
        }
    }

    private void Shoot()
    {
        Instantiate(MarkObject, PointOfWeaponHitObject.transform.position, PointOfWeaponHitObject.transform.rotation,
            MarksRootObject.transform);
    }
}
