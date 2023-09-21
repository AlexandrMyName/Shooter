﻿using System;

namespace EventBus
{
    public static class PlayerEvents
    {
        public static event Action<int> OnGameEnded;
        public static event Action OnGamePaused;
        public static event Action OnDead;
        
        public static void GameEnded(int score)
        {
            OnGameEnded?.Invoke(score);
        }
        
        public static void PauseGame()
        {
            OnGamePaused?.Invoke();
        }
        
        public static void PlayerDead()
        {
            OnDead?.Invoke();
        }
    }
}