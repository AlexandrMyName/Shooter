using UnityEngine;
using Abstracts;
using EventBus;
using System;
using System.Collections.Generic;
using UniRx;
using Cinemachine;
using Configs;
using UnityEngine.InputSystem;

namespace Core
{

    public class PlayerCinemachineSystem : BaseSystem, IDisposable
    {

        private IGameComponents _components;
        private IComponentsStorage _componentsStorage;
        private CinemachineCameraConfig _config;
        private PlayerInput _input;
        private IAnimatorIK _animatorIK;
        List<IDisposable> _disposables = new();

        private float _verticalRecoil;
        private float _recoilDuration;


        protected override void Awake(IGameComponents components)
        {

            _components = components;
            _componentsStorage = _components.BaseObject.GetComponent<IComponentsStorage>();
            _animatorIK = _components.BaseObject.GetComponent<IAnimatorIK>();
            _config = _componentsStorage.CinemachineCameraConfig;
            _input = _componentsStorage.Input.PlayerInput;

            PlayerEvents.OnGamePaused += onPausedGame;
             
            _componentsStorage.RecoilCommand.Subscribe (value =>
            {
                
                OnRecoilWeapon(value);
                 
            }).AddTo(_disposables);
        }


        private void OnRecoilWeapon(WeaponRecoilConfig recoilConfig)
        {

            GenerateCameraRecoil(recoilConfig);
        }


        private void GenerateCameraRecoil(WeaponRecoilConfig recoilConfig)
        {

              _verticalRecoil = recoilConfig.GetRecoilValues().y;
              _recoilDuration = recoilConfig.VerticalRecoilDuration;
              recoilConfig.ImpulseSource.GenerateImpulse(recoilConfig.RecoilVelocity);
        }

        private void onPausedGame(bool isPaused)
        {

            if (isPaused == true)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }


        protected override void Update()
        {

            if (_animatorIK.IsLoseBalance) return;

            if (_recoilDuration > 0)
            {
                float recoilModifier = _input.Player.Aim.IsPressed() ?  0.3f : 0.7f;
                _config.Y_Axis.Value -= (_verticalRecoil/ 10 * Time.deltaTime) / _recoilDuration * recoilModifier;
                //_recoilDuration -= Time.deltaTime;
            }

            if (!_input.Player.Shoot.IsPressed())
            {
                _recoilDuration = 0;
            }

        }


        protected override void FixedUpdate()
        {

            if (_animatorIK.IsLoseBalance) return;

            Vector3 delta = _input.Player.Mouse.ReadValue<Vector2>();
            float x_axis 
                = delta.y *
                (Gamepad.all.Count > 0 
                ? _componentsStorage.CinemachineCameraConfig.Sensetivity_GamePad 
                : _componentsStorage.CinemachineCameraConfig.Sensetivity_Mouse) * Time.fixedDeltaTime;

            float y_axis = delta.x * (Gamepad.all.Count > 0
                ? _componentsStorage.CinemachineCameraConfig.Sensetivity_GamePad
                : _componentsStorage.CinemachineCameraConfig.Sensetivity_Mouse) * Time.fixedDeltaTime;
          
            _componentsStorage.CinemachineCameraConfig.Y_Axis.Value -= x_axis;
            _componentsStorage.CinemachineCameraConfig.X_Axis.Value += y_axis;
            _componentsStorage.CinemachineCameraConfig.Y_Axis.Value
                = Mathf.Clamp(
                    _componentsStorage.CinemachineCameraConfig.Y_Axis.Value,
                    _componentsStorage.CinemachineCameraConfig.Y_AxisRange.x,
                    _componentsStorage.CinemachineCameraConfig.Y_AxisRange.y
            );  

            _componentsStorage.CameraLookAt.transform.eulerAngles
                = new Vector3(
                    _componentsStorage.CinemachineCameraConfig.Y_Axis.Value,
                     _componentsStorage.CinemachineCameraConfig.X_Axis.Value,
                     0);
        }


        public void Dispose()
        => PlayerEvents.OnGamePaused -= onPausedGame;
        
    }
}