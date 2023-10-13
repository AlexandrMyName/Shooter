using Configs;
using Enums;
using EventBus;
using Extentions;
using ShootingSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;


namespace Core
{

    [Serializable]
    public class Muzzle : IDisposable
    {

        //Need refactoring (not faster)
        [Header ("Muzzle behavior"),Space(10)]
        [SerializeField] private WeaponConfig _weaponConfig;
        [SerializeField] private Transform _projectileSpawnTransform;
          
        private Transform _cameraTransform;
        [SerializeField] private GameObject _pointOfCameraHitObject;
        [SerializeField] private GameObject _pointOfWeaponHit;
        private bool _isTryShooting = false;
        private bool _canShoot = false;
        private bool _isReloading = false;
        private bool _isShooting = false;
        private float _lastShootTime;
        private float _spreadingModifier;
        private float _recoilModifierHorizontal;
        private float _recoilModifierVertical;
        [HideInInspector] public int CurrentAmmo;
        [HideInInspector] public int CurrentAmmoInMagazine;
        private int _maxAmmoInBurst;
        private int _currentAmmonBurst;

        private List<IDisposable> _disposables = new();

        private Transform _projectilesPool;
        private Transform _hitEffectsRoot;
        
        private LayerMask _ignoreRaycastLayerMask;

        private bool _isWeaponHitTarget;
        private RaycastHit _weaponHit;


        public void Activate()
        {
             
            ShootingEvents.OnTryShoot += ChangeShootingState;
            ShootingEvents.OnReload += Reload;

            Observable.EveryFixedUpdate().Subscribe(value => FixedUpdate()).AddTo(_disposables);

            ShootingEvents.ChangeAmmoCount(_weaponConfig.MaxAmmo);
            ShootingEvents.ChangeAmmoInMagazineCount(CurrentAmmoInMagazine);

            if (CurrentAmmo > 0) return;

            CurrentAmmo = _weaponConfig.MaxAmmo;
            CurrentAmmoInMagazine = _weaponConfig.MaxAmmoInMagazine;

            if (_weaponConfig.ShootingType == ShootingType.Auto)
            {
                _maxAmmoInBurst = _weaponConfig.MaxAmmoInMagazine;
            }
            else if (_weaponConfig.ShootingType == ShootingType.Burst)
            {
                _maxAmmoInBurst = _weaponConfig.MaxAmmoInBurst;
            }
            else
            {
                _maxAmmoInBurst = 1;
            }

            _currentAmmonBurst = _maxAmmoInBurst;
        }


        public void InitPool(Transform projectilesRoot, Transform hitEffectsRoot, LayerMask ignoreRaycastLayerMask)
        {

            _projectilesPool = projectilesRoot;
            _hitEffectsRoot = hitEffectsRoot;
            _ignoreRaycastLayerMask = ignoreRaycastLayerMask;

            _cameraTransform = Camera.main.transform;
            //_pointOfHitObject =
                GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/Shooting/HitDirection"));
            //_pointOfHitObject.SetActive(false);
            _spreadingModifier = _weaponConfig.SpreadingDefaultModifier;
            _lastShootTime = Time.time;
            ShootingEvents.ChangeAmmoCount(_weaponConfig.MaxAmmo);
            ShootingEvents.ChangeAmmoInMagazineCount(CurrentAmmoInMagazine);

             
        }


        public void Disable()
        {
            ShootingEvents.OnTryShoot -= ChangeShootingState;
            ShootingEvents.OnReload -= Reload;
            _disposables.ForEach(disposable => disposable.Dispose());
        }


        private void ChangeShootingState(bool isShooting)
        {
            _isTryShooting = isShooting;
        }


        private void FixedUpdate()
        {
             
            if (_isTryShooting)
            {
                //ShootingEvents.RotateToCameraDirection(true);
                TryShoot();
               
                ChangeShootingState(false);
            }
            else
            {
                _currentAmmonBurst = _maxAmmoInBurst;
                ChangeSpread(-0.1f);
                ChangeRecoilVector(-_weaponConfig.RecoilModifierDeltaHorizontal,
                    -_weaponConfig.RecoilModifierDeltaVertical);
                //ShootingEvents.RotateToCameraDirection(false);
                if (_isShooting)
                {
                    _isShooting = false;
                    ShootingEvents.Shoot(_isShooting, _weaponConfig.ShootingType, 1f);
                }
            }

        }


        private void Reload()
        {

            if (CurrentAmmo > 0)
            {
                _isReloading = true;
                Observable.FromCoroutine(val => ReloadingAction(_weaponConfig.ReloadDelay)).Subscribe().AddTo(_disposables);
               
            }
        }


        private IEnumerator ReloadingAction(float reloadDelay)
        {

            yield return new WaitForSeconds(reloadDelay);
            if (CurrentAmmo >= _weaponConfig.MaxAmmoInMagazine)
            {
                CurrentAmmo -= _weaponConfig.MaxAmmoInMagazine;
                CurrentAmmo += CurrentAmmoInMagazine;
                CurrentAmmoInMagazine = _weaponConfig.MaxAmmoInMagazine;
            }
            else
            {
                CurrentAmmoInMagazine += CurrentAmmo;
                CurrentAmmo = 0;
                if (CurrentAmmoInMagazine > _weaponConfig.MaxAmmoInMagazine)
                {
                    int deltaAmmo = CurrentAmmoInMagazine - _weaponConfig.MaxAmmoInMagazine;
                    CurrentAmmoInMagazine -= deltaAmmo;
                    CurrentAmmo = deltaAmmo;
                }
            }
            ShootingEvents.ChangeAmmoCount(CurrentAmmo);
            ShootingEvents.ChangeAmmoInMagazineCount(CurrentAmmoInMagazine);
            _isReloading = false;
            yield return null;
        }


        private void TryShoot()
        {

            bool haveAmmoInMagazine;
            bool haveAmmoInBurst; 
            //if (_weaponConfig.ShootingType != ShootingType.Auto) ??
            //{
            //    haveAmmoInBurst = _currentAmmonBurst > 0;
            //}
            //else ??
            //{
            //    haveAmmoInBurst = true;
            //}

             haveAmmoInBurst = true;

            if (_weaponConfig.MaxAmmo > 0)
            {
                haveAmmoInMagazine = CurrentAmmoInMagazine > 0;
            }
            else
            {
                haveAmmoInMagazine = true;
            }
            _canShoot = (_lastShootTime + _weaponConfig.ShootDelay < Time.time)
                        && haveAmmoInBurst && haveAmmoInMagazine && !_isReloading;
            
            if (_canShoot)
            {
                _currentAmmonBurst--;
                CurrentAmmoInMagazine--;
                _lastShootTime = Time.time;
                ShootingEvents.ChangeAmmoInMagazineCount(CurrentAmmoInMagazine);

                Shoot();
                if (!_isShooting)
                {
                    _isShooting = true;
                    ShootingEvents.Shoot(_isShooting, _weaponConfig.ShootingType, 1f);
                }

            }
            else if (_isShooting && (!haveAmmoInBurst || !haveAmmoInMagazine || _isReloading))
            {
                _isShooting = false;
                ShootingEvents.Shoot(_isShooting, _weaponConfig.ShootingType, 1f);
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
            MovePointOfHit();

            
            GameObject projectile = GameObject.Instantiate(_weaponConfig.ProjectilePrefab,
                _projectileSpawnTransform.position, _projectileSpawnTransform.rotation, _projectilesPool);
            Projectile projectileView = projectile.GetOrAddComponent<Projectile>();
            projectileView.IsWeaponHitTarget = _isWeaponHitTarget;
            projectileView.Hit = _weaponHit;
            projectileView.StartMoving(_projectileSpawnTransform.position, _pointOfWeaponHit.transform.position,
                _hitEffectsRoot);
            //TrailView trailView = projectile.GetOrAddComponent<TrailView>();
            //trailView.TrailRenderer.AddPosition(_pointOfWeaponHit.transform.position);
            //trailView.TrailRenderer.OnCollisionEnterAsObservable().Subscribe(
            //    val => HitObject(val)).AddTo(_disposables);
            

        }

        private void HitObject(Collision collision)
        {
            Debug.Log(collision.gameObject.name);
        }

        private void MovePointOfHit()
        {
            RaycastHit cameraHit;
            RaycastHit weaponHit;

            LayerMask layerMask = ~_ignoreRaycastLayerMask;

            bool isCameraRayHitTarget = Physics.Raycast(_cameraTransform.position,
                _cameraTransform.TransformDirection(Vector3.forward), out cameraHit,
                Mathf.Infinity, layerMask);
            if (isCameraRayHitTarget)
            {
                MoveHitMarkObject(_pointOfCameraHitObject, cameraHit.point, cameraHit.normal);
                _projectileSpawnTransform.LookAt(_pointOfCameraHitObject.transform);
            }
            else
            {
                Vector3 dir = _cameraTransform.position + (_cameraTransform.TransformDirection(Vector3.forward) * 1000);
                MoveHitMarkObject(_pointOfCameraHitObject, dir, Vector3.forward);
                _projectileSpawnTransform.LookAt(_pointOfCameraHitObject.transform);
            }
            Vector3 direction = GetRecoilVector();
            bool isWeaponRayHitTarget = Physics.Raycast(_projectileSpawnTransform.position,
                _projectileSpawnTransform.TransformDirection(direction), out weaponHit,
                Mathf.Infinity, layerMask);
            if (isWeaponRayHitTarget)
            {
                MoveHitMarkObject(_pointOfWeaponHit, weaponHit.point, weaponHit.normal);
                _weaponHit = weaponHit;
            }
            else
            {
                Vector3 dir = _projectileSpawnTransform.position +
                              (_projectileSpawnTransform.TransformDirection(Vector3.forward) * 1000);
                MoveHitMarkObject(_pointOfWeaponHit, dir, Vector3.forward);
            }
            _isWeaponHitTarget = isWeaponRayHitTarget;
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


        private void ChangeRecoilVector(float recoilHorizontalDelta, float recoilVerticalDelta)
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


        public void Dispose()
        => _disposables.ForEach(disposable => disposable.Dispose());
        
    }
}