using Configs;
using Enums;
using EventBus;
using Extentions;
using ShootingSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;


namespace Core
{

    [Serializable]
    public class Muzzle : IDisposable
    {
        
       
        [Header ("Muzzle behavior"),Space(10)]
        [SerializeField] private WeaponConfig _weaponConfig;
        [SerializeField] private Transform _muzzleRoot;

        [SerializeField] private BulletConfig _bulletConfig;
        private RaycastWeapon _weaponRay;

        private ComponentsStorage _baseComponents;
        private WeaponRecoilConfig _recoilConfig;
        private bool _isTryShooting = false;
        private bool _canShoot = false;
        private bool _isReloading = false;
        private bool _isShooting = false;
        private float _lastShootTime;
        private float _spreadingModifier;
        private float _recoilModifierHorizontal;
        private float _recoilModifierVertical;
        private int _currentAmmo;
        private int _currentAmmoInMagazine;
        private int _maxAmmoInBurst;
        private int _currentAmmonBurst;

        private List<IDisposable> _disposables = new();
       
        private ParticleSystem[] _muzzleFlash;

        public int CurrentAmmo => _currentAmmo;
        public int CurrentAmmoInMagazine => _currentAmmoInMagazine;
        
         
        public void Activate()
        {
             
            ShootingEvents.OnTryShoot += ChangeShootingState;
            ShootingEvents.OnReload += Reload;
            PlayerEvents.OnRifleAmmoAdded += AddAmmo;

            Observable.EveryFixedUpdate().Subscribe(value => FixedUpdate()).AddTo(_disposables);
            Observable.EveryUpdate().Subscribe(value => Update()).AddTo(_disposables);

            if (_currentAmmo > 0) return;

            _currentAmmo = _weaponConfig.MaxAmmo;
            _currentAmmoInMagazine = _weaponConfig.MaxAmmoInMagazine;

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
        

        public void InitPool(
            LayerMask ignoreRaycastLayerMask,
            ParticleSystem[] muzzleFlash ,
            Transform crossHairTransform,
            ParticleSystem hitEffect, // this not correct (need create new config with array and material's type)
            WeaponRecoilConfig recoilConfig,
            ComponentsStorage baseComponents
            ){ ///INITIALIZER\\\

            _baseComponents = baseComponents;
            _recoilConfig = recoilConfig;
            _muzzleFlash = muzzleFlash;
            _weaponRay = new RaycastWeapon(
                crossHairTransform,
                _muzzleRoot,
                ignoreRaycastLayerMask,
                hitEffect);
  
            _spreadingModifier = _weaponConfig.SpreadingDefaultModifier;
            _lastShootTime = Time.time;
        }


        public void Disable()
        {

            ShootingEvents.OnTryShoot -= ChangeShootingState;
            ShootingEvents.OnReload -= Reload;
            PlayerEvents.OnRifleAmmoAdded -= AddAmmo;
            _disposables.ForEach(disposable => disposable.Dispose());
        }


        private void ChangeShootingState(bool isShooting) => _isTryShooting = isShooting;
        

        void FixedUpdate()
        {

            ShootingEvents.ChangeAmmoCount(_currentAmmo);
            ShootingEvents.ChangeAmmoInMagazineCount(_currentAmmoInMagazine);
             
            if (_isTryShooting)
            {
                TryShoot();
               
                ChangeShootingState(false);
            }
            else
            {
                _currentAmmonBurst = _maxAmmoInBurst;
                ChangeSpread(-0.1f);
             
                if (_isShooting)
                {
                    _isShooting = false;
                    ShootingEvents.Shoot(_isShooting, _weaponConfig.ShootingType, 1f);
                }
            }

        }


        void Update()
        {
            _weaponRay.UpdateBullets(Time.deltaTime);
        }

        private void Reload()
        {

            if (_currentAmmo > 0)
            {
                _isReloading = true;
                Observable.FromCoroutine(val => ReloadingAction(_weaponConfig.ReloadDelay)).Subscribe().AddTo(_disposables);
               
            }
        }
        

        private void AddAmmo(int additionAmmo, PickUp.CallBack callback)
        {

            if (_currentAmmo == _weaponConfig.MaxAmmo)
            {
                return;
            }
            if (_currentAmmo + additionAmmo > _weaponConfig.MaxAmmo)
            {
                _currentAmmo = _weaponConfig.MaxAmmo;
            }
            else
            {
                _currentAmmo += additionAmmo;
            }
            callback();
        }


        private IEnumerator ReloadingAction(float reloadDelay)
        {

            yield return new WaitForSeconds(reloadDelay);

            if (_currentAmmo >= _weaponConfig.MaxAmmoInMagazine)
            {
                _currentAmmo -= _weaponConfig.MaxAmmoInMagazine;
                _currentAmmo += _currentAmmoInMagazine;
                _currentAmmoInMagazine = _weaponConfig.MaxAmmoInMagazine;
            }
            else
            {
                _currentAmmoInMagazine += _currentAmmo;
                _currentAmmo = 0;
                if (_currentAmmoInMagazine > _weaponConfig.MaxAmmoInMagazine)
                {
                    int deltaAmmo = _currentAmmoInMagazine - _weaponConfig.MaxAmmoInMagazine;
                    _currentAmmoInMagazine -= deltaAmmo;
                    _currentAmmo = deltaAmmo;
                }
            }
            ShootingEvents.ChangeAmmoCount(_currentAmmo);
            ShootingEvents.ChangeAmmoInMagazineCount(_currentAmmoInMagazine);
            _isReloading = false;
            yield return null;
        }


        private void TryShoot()
        {

            bool haveAmmoInMagazine = _weaponConfig.MaxAmmo > 0 ? _currentAmmoInMagazine > 0  :  true; ;
            bool haveAmmoInBurst = true;
             
            _canShoot = 
                (_lastShootTime + _weaponConfig.ShootDelay < Time.time)
                 && haveAmmoInBurst && haveAmmoInMagazine && !_isReloading;
            

            if (_canShoot)
            {

                _currentAmmonBurst--;
                _currentAmmoInMagazine--;
                _lastShootTime = Time.time;
                ShootingEvents.ChangeAmmoInMagazineCount(_currentAmmoInMagazine);

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

            _baseComponents.Recoil.Value += _recoilConfig.GetRecoil() + Vector3.up * Time.deltaTime;
            foreach (var effect in _muzzleFlash)
            {
                effect.Emit(1);
            }
            if (_bulletConfig == null)
                throw new NullReferenceException("Bullet config is null => (Muzzle) on WeaponData");
             
            _weaponRay.Fire(_bulletConfig);
              
            if (_recoilModifierHorizontal >=
                _weaponConfig.RecoilHorizontal && 
                _recoilModifierVertical >= _weaponConfig.RecoilVertical) {
                ChangeSpread(_weaponConfig.SpreadingModifierDelta);
            }
            
        }
 
         
        private Vector3 GetRecoilVector()
        {
            float maxSpreadX = 0.06f;
            float maxSpreadY = 0.06f;
            
            Vector3 recoiledDirection = Vector3.forward;

            float x = _recoilModifierHorizontal;
            float y = _recoilModifierVertical;

            float spreadX = Extention.GetRandomFloat(-_weaponConfig.SpreadingHorizontal,
                _weaponConfig.SpreadingHorizontal) * _spreadingModifier;
            float spreadY = Extention.GetRandomFloat(-_weaponConfig.SpreadingVertical,
                _weaponConfig.SpreadingVertical) * _spreadingModifier;
            recoiledDirection = new Vector3(x + maxSpreadX * spreadX, y + maxSpreadY * spreadY, 1);
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
            float maxSpreadDelta = 0.06f;
            
            bool isSpreadIncreasing = spreadDelta > 0 && _spreadingModifier < 1;
            bool isSpreadDecreasing = spreadDelta < 0 && _spreadingModifier > _weaponConfig.SpreadingDefaultModifier;
            if (isSpreadIncreasing || isSpreadDecreasing)
            {
                _spreadingModifier += spreadDelta * maxSpreadDelta;
            }
        }


        public void Dispose()
        => _disposables.ForEach(disposable => disposable.Dispose());
        
    }
}