using Abstracts;
using System;
using System.Collections.Generic;
using UnityEngine;


namespace Core
{

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
            
        }


        private void SwitchWeapon(IWeaponType weaponID)
        {

            _animatorIK.SetWeaponState(weaponID);

        }
    }
}