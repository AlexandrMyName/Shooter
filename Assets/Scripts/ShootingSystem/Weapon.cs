using Assets.Scripts.Enums;
using Configs;
using EventBus;
using Extentions;
using UnityEngine;
using System.Collections;

namespace ShootingSystem
{
    public class Weapon : MonoBehaviour
    {
        [SerializeField] private Transform _playerObjectTransform;
        [SerializeField] private WeaponConfig _weaponConfig;
        [SerializeField] private Transform _projectileSpawnTransform;
        [SerializeField] private Transform _projectilesPool;
        [SerializeField] private Transform _hitEffectsRoot;
        [SerializeField] private int _playerLayerIndex = 8;
        [SerializeField] private int _playerRagdollLayerIndex = 9;

        private Transform _cameraTransform;
        private GameObject _pointOfHitObject;
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

        private void OnEnable()
        {
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

            ShootingEvents.OnTryShoot += ChangeShootingState;
            ShootingEvents.OnReload += Reload;
        }

        private void Start()
        {
            _cameraTransform = Camera.main.transform;
            _pointOfHitObject =
                GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/Shooting/HitDirection"));
            _pointOfHitObject.SetActive(false);
            _spreadingModifier = _weaponConfig.SpreadingDefaultModifier;
            _lastShootTime = Time.time;
            ShootingEvents.ChangeAmmoCount(_weaponConfig.MaxAmmo);
            ShootingEvents.ChangeAmmoInMagazineCount(_currentAmmoInMagazine);
        }

        private void OnDisable()
        {
            ShootingEvents.OnTryShoot -= ChangeShootingState;
            ShootingEvents.OnReload -= Reload;
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
            if (_currentAmmo > 0)
            {
                _isReloading = true;
                StartCoroutine(ReloadingAction(_weaponConfig.ReloadDelay));
            }
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
            bool haveAmmoInMagazine;
            bool haveAmmoInBurst;
            if (_weaponConfig.ShootingType != ShootingType.Auto)
            {
                haveAmmoInBurst = _currentAmmonBurst > 0;
            }
            else
            {
                haveAmmoInBurst = true;
            }
            if (_weaponConfig.MaxAmmo > 0)
            {
                haveAmmoInMagazine = _currentAmmoInMagazine > 0;
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
            projectileView.StartMoving(_projectileSpawnTransform.position, _pointOfHitObject.transform.position,
                _hitEffectsRoot);
        }

        private void MovePointOfHit()
        {
            RaycastHit cameraHit;
            RaycastHit weaponHit;

            LayerMask layerMask = ~(1 << _playerLayerIndex | 1 << _playerRagdollLayerIndex);

            bool isCameraRayHitTarget = Physics.Raycast(_cameraTransform.position,
                _cameraTransform.TransformDirection(Vector3.forward), out cameraHit,
                Mathf.Infinity, layerMask);
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
                Mathf.Infinity, layerMask);
            if (isWeaponRayHitTarget)
            {
                MoveHitMarkObject(_pointOfHitObject, weaponHit.point, weaponHit.normal);
            }
            else
            {
                Vector3 dir = _projectileSpawnTransform.position + (_projectileSpawnTransform.TransformDirection(Vector3.forward) * 1000);
                MoveHitMarkObject(_pointOfHitObject, dir, Vector3.forward);
            }
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