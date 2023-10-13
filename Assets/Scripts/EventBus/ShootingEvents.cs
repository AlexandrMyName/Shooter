using System;
using System.Net.NetworkInformation;
using Enums;
using UnityEngine;

namespace EventBus
{
    public static class ShootingEvents
    {
        public static event Action<bool> OnTryShoot;
        public static event Action<bool, ShootingType, float> OnShoot;
        public static event Action<bool> OnCameraDirectionRotate; 
        public static event Action<bool> OnAim;
        public static event Action OnReload;
        public static event Action<int> OnAmmoCountChanged;
        public static event Action<int> OnAmmoCountInMagazineChanged;

        public static void TryShoot(bool isTryShooting)
        {
            OnTryShoot?.Invoke(isTryShooting);
        }

        public static void Shoot(bool isShooting, ShootingType shootingType, float animationSpeed)
        {
            OnShoot?.Invoke(isShooting, shootingType, animationSpeed);
        }

        public static void RotateToCameraDirection(bool isRotating)
        {
            OnCameraDirectionRotate?.Invoke(isRotating);
        }

        public static void Aim(bool isAiming)
        {
            OnAim?.Invoke(isAiming);
        }

        public static void Reload()
        {
            OnReload?.Invoke();
        }

        public static void ChangeAmmoCount(int ammoCount)
        {
            OnAmmoCountChanged?.Invoke(ammoCount);
        }

        public static void ChangeAmmoInMagazineCount(int ammoCount)
        {
            OnAmmoCountInMagazineChanged?.Invoke(ammoCount);
        }
        
    }
}