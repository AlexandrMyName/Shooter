using Configs;
using Extentions;
using UnityEditor;
using UnityEngine;

namespace ShootingSystem
{
    public class Weapon : MonoBehaviour
    {
        [SerializeField] private KeyCode _shootKeyCode = KeyCode.Mouse0;
        [SerializeField] private WeaponConfig _weaponConfig;
        [SerializeField] private Transform _projectileSpawnTransform;
        [SerializeField] private Transform _projectilesPool;
        
        private Transform _cameraTransform;
        private GameObject _pointOfHitObject;
        private bool _isShooting = false;
        private float _lastShootTime;
        private float _spreadingModifier;
        private float _recoilModifierHorizontal;
        private float _recoilModifierVertical;

        private void Start()
        {
            _cameraTransform = Camera.main.transform;
            _pointOfHitObject = 
                GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/Shooting/HitDirection"));
            _pointOfHitObject.SetActive(false);
            _spreadingModifier = _weaponConfig.SpreadingDefaultModifier;
        }
        
        private void Update() 
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
            if (_isShooting)
            {
                TryShoot();
            }
            else
            {
                ChangeSpread(-0.1f);
                ChangeRecoilVector(-_weaponConfig.RecoilModifierDeltaHorizontal, 
                    -_weaponConfig.RecoilModifierDeltaVertical);
            }
            
        }

        private void TryShoot()
        {
            if (_lastShootTime + _weaponConfig.ShootDelay < Time.time)
            {
                Shoot();
                _lastShootTime = Time.time;
            }
        }
        
        private void Shoot()
        {
            if (_recoilModifierHorizontal >= 
                _weaponConfig.RecoilHorizontal && _recoilModifierVertical >= _weaponConfig.RecoilVertical)
            {
                ChangeSpread(_weaponConfig.SpreadingModifierDelta);
            }
            ChangeRecoilVector(_weaponConfig.RecoilModifierDeltaHorizontal, _weaponConfig.RecoilModifierDeltaVertical);
            
            
            RaycastHit cameraHit;
            RaycastHit weaponHit;
            
            bool isCameraRayHitTarget = Physics.Raycast(_cameraTransform.position,
                _cameraTransform.TransformDirection(Vector3.forward), out cameraHit,
                Mathf.Infinity);
            if (isCameraRayHitTarget)
            {
                MoveHitMarkObject(_pointOfHitObject, cameraHit.point, cameraHit.normal);
                _projectileSpawnTransform.LookAt(_pointOfHitObject.transform);
            }
            else
            {
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
                hitedObject = weaponHit.collider.gameObject;
                MoveHitMarkObject(_pointOfHitObject, weaponHit.point, weaponHit.normal);
            }
            else
            {

                MoveHitMarkObject(_pointOfHitObject, weaponHit.point, weaponHit.normal);
            }

            
            

            GameObject projectile = GameObject.Instantiate(_weaponConfig.ProjectilePrefab,
                _projectileSpawnTransform.position ,_projectileSpawnTransform.rotation ,_projectilesPool);
            _pointOfHitObject.SetActive(true);
            Debug.Log("Shoot");
        }
        
        private void MoveHitMarkObject(GameObject markObject, Vector3 rayHitPoint, Vector3 rayHitNormal)
        {
            markObject.transform.position = rayHitPoint;
            markObject.transform.rotation = Quaternion.LookRotation(rayHitNormal);
            markObject.SetActive(true);
        }
        
        private Vector3 GetRecoilVector()
        {
            Vector3 recoiledDirection = Vector3.forward;

            float x = _recoilModifierHorizontal;
            float y = _recoilModifierVertical;
        
            float spreadX = Extention.GetRandomFloat(-_weaponConfig.SpreadingHorizontal,
                _weaponConfig.SpreadingHorizontal) * _spreadingModifier;
            float spreadY = Extention.GetRandomFloat(-_weaponConfig.SpreadingVertical,
                _weaponConfig.SpreadingVertical) * _spreadingModifier;
            recoiledDirection = new Vector3(x + spreadX, y + spreadY, 1);
            return recoiledDirection;
        }
        
        private void ChangeRecoilVector(float recoilHorizontalDelta,float recoilVerticalDelta)
        {
            bool isRecoilHorizontalIncreasing = 
                _recoilModifierHorizontal < _weaponConfig.RecoilHorizontal && recoilHorizontalDelta > 0;
            bool isRecoilHorizontalDecreasing = _recoilModifierHorizontal > 0 && recoilHorizontalDelta < 0;
            if (isRecoilHorizontalIncreasing || isRecoilHorizontalDecreasing)
            {
                _recoilModifierHorizontal += recoilHorizontalDelta;
            }

            bool isRecoilVerticalIncreasing = 
                _recoilModifierVertical < _weaponConfig.RecoilVertical && recoilVerticalDelta > 0;
            bool isRecoilVerticalDecreasing = _recoilModifierVertical > 0 && recoilVerticalDelta < 0;
            if (isRecoilVerticalIncreasing || isRecoilVerticalDecreasing)
            {
                _recoilModifierVertical += recoilVerticalDelta;
            }
        }

        private void ChangeSpread(float spreadDelta)
        {
            bool isSpreadIncreasing = spreadDelta > 0 && _spreadingModifier < 1;
            bool isSpreadDecreasing = spreadDelta < 0 && _spreadingModifier > _weaponConfig.SpreadingDefaultModifier;
            if (isSpreadIncreasing || isSpreadDecreasing)
            {
                _spreadingModifier += spreadDelta;
            }
        }
        
    }
}