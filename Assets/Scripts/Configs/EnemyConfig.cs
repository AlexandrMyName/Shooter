using Enums;
using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(fileName = "EnemyConfig", menuName = "Configs/EnemySystem/EnemyConfig", order = 0)]
    public class EnemyConfig : ScriptableObject
    {
        [SerializeField] private int _enemyHP;
        [SerializeField] private EnemyAttackType _attackType;
        [SerializeField] private int _enemyDamage;
        [SerializeField] private int _enemyAttackDelay;
        [SerializeField] private int _shootingProjectileSpeed;
        [SerializeField] private GameObject _shootingProjectile;
        
        public int EnemyHp => _enemyHP;

        public EnemyAttackType AttackType => _attackType;

        public int EnemyDamage => _enemyDamage;

        public int EnemyAttackDelay => _enemyAttackDelay;

        public int ShootingProjectileSpeed => _shootingProjectileSpeed;

        public GameObject ShootingProjectile => _shootingProjectile;
    }
}