using System;

namespace EventBus
{
    public static class PlayerEvents
    {
        public static event Action<int> OnPlayerSpawned;
        public static event Action<int> OnPlayerDamaged;
        public static event Action<int> OnGameEnded;
        public static event Action<bool> OnGamePaused;
        public static event Action OnDead;

        public static void SpawnPlayer(int hp)
        {
            OnPlayerSpawned?.Invoke(hp);
        }
        
        public static void DamagePlayer(int hp)
        {
            OnPlayerDamaged?.Invoke(hp);
        }

        public static void GameEnded(int score)
        {
            OnGameEnded?.Invoke(score);
        }
        
        public static void PauseGame(bool isPaused)
        {
            OnGamePaused?.Invoke(isPaused);
        }
        
        public static void PlayerDead()
        {
            OnDead?.Invoke();
        }
    }
}