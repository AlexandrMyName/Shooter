using Abstracts;
using EventBus;
using System;
using System.Collections.Generic;
using UnityEngine;


namespace Core
{
    /// <summary>
    /// Need add Input System with out every update
    /// </summary>
    public class PlayerShootSystem : BaseSystem, IDisposable
    {

        IGameComponents _components;
        IAnimatorIK _animatorIK;
        private List<IDisposable> _disposables = new();

        private IWeaponType _weaponType;


        protected override void Awake(IGameComponents components)
        {

            _animatorIK = components.BaseObject.GetComponent<IPlayer>().ComponentsStorage.AnimatorIK;

        }


        public void Dispose()
        {
            
        }


        protected override void Update()
        {

            if (Input.GetKeyDown(KeyCode.Alpha0))
            {
                _weaponType = IWeaponType.None;
                SwitchWeapon(_weaponType);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                _weaponType = IWeaponType.Pistol;
                SwitchWeapon(_weaponType);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                _weaponType = IWeaponType.Auto;
                SwitchWeapon(_weaponType);
            }


            if (Input.GetMouseButton(0))
            {
                if(Input.GetMouseButton(1))
                    ShootingEvents.TryShoot(true);
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                ShootingEvents.Reload();
            }
        }


        private void SwitchWeapon(IWeaponType weaponID)
        {

            _animatorIK.SetWeaponState(weaponID);

        }
    }
}