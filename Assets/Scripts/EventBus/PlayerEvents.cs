using System;

namespace EventBus
{
    public static class PlayerEvents
    {
        public static event Action<int> OnPlayerSpawned;
        public static event Action<int> OnUpdateHealthView;
        public static event Action<int, int> OnUpdateArmorView;
        public static event Action<int> OnGameEnded;
        public static event Action<int, Healing.CallBack> OnPlayerHealed;
        public static event Action<int, Armor.CallBack> OnPlayerArmorAdded;
        public static event Action<bool> OnGodMode;
        public static event Action<bool> OnGamePaused;
        public static event Action<bool> OnKeyStatusChanged;
        
        public static event Action OnDead;

        public static void SpawnPlayer(int hp)
        {
            OnPlayerSpawned?.Invoke(hp);
        }
        
        public static void UpdateHealthView(int hp)
        {
            OnUpdateHealthView?.Invoke(hp);
        }
        public static void UpdateArmorView(int armor, int maxArmor)
        {
            OnUpdateArmorView?.Invoke(armor, maxArmor);
        }
        public static void HealPlayer(int hp, Healing.CallBack callBack)
        {
            OnPlayerHealed?.Invoke(hp, callBack);
        }
        public static void AddArmor(int armor, Armor.CallBack callBack)
        {
            OnPlayerArmorAdded?.Invoke(armor, callBack);
        }
        public static void GodMode(bool isGodModeEnabled)
        {
            OnGodMode?.Invoke(isGodModeEnabled);
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

        public static void ChangeKeyStatus(bool hasKey)
        {
            OnKeyStatusChanged?.Invoke(hasKey);
        }
    }
}