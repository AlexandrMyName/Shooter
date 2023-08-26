using System;
using Assets.Scripts.Enums;
using UnityEngine;

namespace EventBus
{
    public static class ShootingEvents
    {
        public static event Action<bool> OnTryShoot;
        public static event Action<bool, ShootingType, float> OnShoot;
        public static event Action<bool> OnAim;
        public static event Action OnReload;

        public static void TryShoot(bool isTryShooting)
        {
            OnTryShoot?.Invoke(isTryShooting);
        }

        public static void Shoot(bool isShooting, ShootingType shootingType, float animationSpeed)
        {
            OnShoot?.Invoke(isShooting, shootingType, animationSpeed);
        }

        public static void Aim(bool isAiming)
        {
            OnAim?.Invoke(isAiming);
        }

        public static void Reload()
        {
            OnReload?.Invoke();
        }
    }
}