using System;

namespace EventBus
{
    public static class ShootingEvents
    {
        public static event Action<bool> OnShoot;
        public static event Action<bool> OnAim;
        public static event Action OnReload;

        public static void Shoot(bool isShooting)
        {
            OnShoot?.Invoke(isShooting);
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