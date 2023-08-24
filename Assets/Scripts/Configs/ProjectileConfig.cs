using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(fileName = "ProjectileConfig", menuName = "Configs/WeaponSystem/ProjectileConfig", order = 1)]
    public class ProjectileConfig : ScriptableObject
    {
        [SerializeField] private int _damage;
        [SerializeField] float _maxDistance;
        [SerializeField] private float _projectileSpeed;

        public int Damage => _damage;

        public float MaxDistance => _maxDistance;

        public float ProjectileSpeed => _projectileSpeed;
    }
}