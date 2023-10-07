using RootMotion.Dynamics;
using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(fileName = "ProjectileConfig", menuName = "Configs/WeaponSystem/ProjectileConfig", order = 1)]
    public class ProjectileConfig : ScriptableObject
    {
        [SerializeField] private int _damage;
        [SerializeField] float _maxLifetime;
        [SerializeField] float _damageRadius = 0.05f;
        [SerializeField] private float _projectileSpeed;
        [SerializeField] private bool _isMadeImpact;
        [SerializeField] private bool _isGrenade;
        [SerializeField] private ParticleSystem _impactParticleSystem;


        [field: SerializeField] public ProjectileType ProjectileType { get; private set; }
        [field: SerializeField] public DamageType DamageType { get; private set; }
        [field: SerializeField] public float Force { get; private set; }
        [field: SerializeField] public float UpwardsModifier { get; private set; }


        public int Damage => _damage;
        public float MaxLifetime => _maxLifetime;
        public float DamageRadius => _damageRadius;
        public float ProjectileSpeed => _projectileSpeed;
        public bool IsMadeImpact => _isMadeImpact;
        public bool IsGrenade => _isGrenade;
        public ParticleSystem ImpactParticleSystem => _impactParticleSystem;
        
    }
}