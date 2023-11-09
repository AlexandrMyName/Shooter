using Enums;
using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(fileName = "EnemyConfig", menuName = "Configs/EnemySystem/EnemyConfig", order = 0)]
    public class EnemyConfig : ScriptableObject
    {
        [SerializeField] private int _enemyHP;
        [SerializeField] private float _stunPossibility;
        [SerializeField] private float _stunTime;
        [SerializeField] private EnemyAttackType _attackType;
        [SerializeField] private int _enemyDamage;
        [SerializeField] private float _enemyAttackDelay;
        [SerializeField] private float _enemyAttackDuration;
        [SerializeField] private int _shootingProjectileSpeed;
        [SerializeField] private float _attackDistance;
        [SerializeField] private float _meleeDistance;
        [SerializeField] private GameObject _shootingProjectile;
        
        public int EnemyHp => _enemyHP;

        public float StunPossibility => _stunPossibility;

        public float StunTime => _stunTime;

        public EnemyAttackType AttackType => _attackType;

        public int EnemyDamage => _enemyDamage;

        public float EnemyAttackDelay => _enemyAttackDelay;

        public float EnemyAttackDuration => _enemyAttackDuration;

        public int ShootingProjectileSpeed => _shootingProjectileSpeed;

        public float AttackDistance => _attackDistance;
        public float MeleeDistance => _meleeDistance;

        public GameObject ShootingProjectile => _shootingProjectile;
    }
}