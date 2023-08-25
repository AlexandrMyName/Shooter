using Assets.Scripts.Enums;
using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(fileName = "WeaponConfig", menuName = "Configs/WeaponSystem/WeaponConfig", order = 0)]
    public class WeaponConfig : ScriptableObject
    {
        [SerializeField] private ShootingType _shootingType;

        [SerializeField] private int _maxAmmo;
        [SerializeField] private int _maxAmmoInMagazine;
        [SerializeField] private int _maxAmmoInBurst;

        [SerializeField] private float _shootDelay;
        [SerializeField] private float _burstDelay;
        [SerializeField] private float _reloadDelay;

        [SerializeField] private float _aimingSpeed;
        [SerializeField] private float _zoomMultiplicity;
        
        [SerializeField] private float _spreadingHorizontal;
        [SerializeField] private float _spreadingVertical;
        [SerializeField] private float _spreadingModifierDelta;
        [SerializeField] private float _spreadingDefaultModifier;
        [SerializeField] private float _aimingSpreadingModifier;
        
        [SerializeField] private float _recoilHorizontal;
        [SerializeField] private float _recoilVertical;
        [SerializeField] private float _recoilModifierDeltaHorizontal;
        [SerializeField] private float _recoilModifierDeltaVertical;
        [SerializeField] private float _aimingRecoilModifier;

        [SerializeField] private GameObject _projectilePrefab;
        [SerializeField] private TrailRenderer _projectileTrail;
        [SerializeField] private ParticleSystem _impactParticleSystem;
        [SerializeField] private ParticleSystem _shootingParticleSystem;
        
        
        public ShootingType ShootingType => _shootingType;
        
        public int MaxAmmo => _maxAmmo;

        public int MaxAmmoInMagazine => _maxAmmoInMagazine;
        
        public int MaxAmmoInBurst => _maxAmmoInBurst;

        public float ShootDelay => _shootDelay;
        
        public float BurstDelay => _burstDelay;
        
        public float ReloadDelay => _reloadDelay;

        public float AimingSpeed => _aimingSpeed;

        public float ZoomMultiplicity => _zoomMultiplicity;

        public float SpreadingHorizontal => _spreadingHorizontal;

        public float SpreadingVertical => _spreadingVertical;

        public float SpreadingModifierDelta => _spreadingModifierDelta;

        public float SpreadingDefaultModifier => _spreadingDefaultModifier;

        public float AimingSpreadingModifier => _aimingSpreadingModifier;
        
        public float RecoilHorizontal => _recoilHorizontal;

        public float RecoilVertical => _recoilVertical;

        public float RecoilModifierDeltaHorizontal => _recoilModifierDeltaHorizontal;

        public float RecoilModifierDeltaVertical => _recoilModifierDeltaVertical;
        
        public float AimingRecoilModifier => _aimingRecoilModifier;

        public GameObject ProjectilePrefab => _projectilePrefab;

        public TrailRenderer ProjectileTrail => _projectileTrail;

        public ParticleSystem ImpactParticleSystem => _impactParticleSystem;

        public ParticleSystem ShootingParticleSystem => _shootingParticleSystem;
    }
}