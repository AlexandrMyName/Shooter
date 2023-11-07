using Abstracts;
using EventBus;
using System;
using System.Collections.Generic;


namespace Core
{
    /// <summary>
    /// Need add Input System with out every update
    /// </summary>
    public class PlayerShootSystem : BaseSystem, IDisposable
    {

        IGameComponents _components;
        IAnimatorIK _animatorIK;
        private PlayerInput _input;
        
        private List<IDisposable> _disposables = new();

        private IWeaponType _weaponType;


        protected override void Awake(IGameComponents components)
        {
            _components = components;
            _input = components.BaseObject.GetComponent<IComponentsStorage>().Input.PlayerInput;
            _animatorIK = components.BaseObject.GetComponent<IPlayer>().ComponentsStorage.AnimatorIK;

        }


        public void Dispose()
        {
            
        }


        protected override void Update()
        {

            if (_animatorIK.IsLoseBalance) return;

            if (_input.Player.Weapon1.IsPressed())
            {
                _weaponType = IWeaponType.Pistol;
                SwitchWeapon(_weaponType);
            }
            else if (_input.Player.Weapon2.IsPressed())
            {
                _weaponType = IWeaponType.Auto;
                SwitchWeapon(_weaponType);
            }
            else if (_input.Player.Weapon3.IsPressed())
            {
                _weaponType = IWeaponType.Rocket;
                SwitchWeapon(_weaponType);
            }
             
            if (_input.Player.Shoot.IsPressed())
            {
                if(_input.Player.Aim.IsPressed())
                    ShootingEvents.TryShoot(true);
            }

          
        }


        private void SwitchWeapon(IWeaponType weaponID)
        {

            _animatorIK.SetWeaponState(weaponID);

        }
    }
}