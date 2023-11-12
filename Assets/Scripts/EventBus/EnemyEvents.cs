using System;

namespace EventBus
{
    public static class EnemyEvents
    {
        public static event Action OnDamaged;
        public static event Action OnDead;
        public static event Action<bool> OnBossStateChanged;
        public static event Action<int> OnBossSpawnTimerChanged;
        public static event Action<float, float> OnBossHPUpdated; 

        public static void EnemyDamaged()
        {
            OnDamaged?.Invoke();
        }
        
        public static void EnemyDead()
        {
            OnDead?.Invoke();
        }

        public static void ChangeBossState(bool isAlive)
        {
            OnBossStateChanged?.Invoke(isAlive);
        }
        
        public static void ChangeBossSpawningTimer(int timeToSpawn)
        {
            OnBossSpawnTimerChanged?.Invoke(timeToSpawn);
        }

        public static void UpdateBossHP(float hp, float maxHP)
        {
            OnBossHPUpdated?.Invoke(hp, maxHP);
        }


    }
}