using System;

namespace EventBus
{
    public static class EnemyEvents
    {
        public static event Action OnDamaged;
        public static event Action OnDead;
        
        public static void EnemyDamaged()
        {
            OnDamaged?.Invoke();
        }
        
        public static void EnemyDead()
        {
            OnDead?.Invoke();
        }
    }
}