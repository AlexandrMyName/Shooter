using UnityEngine;
using Abstracts;
using EventBus;
using System;
using System.Collections.Generic;
using UniRx;
using Cinemachine;
using Configs;

namespace Core
{

    public class PlayerCinemachineSystem : BaseSystem, IDisposable
    {

        private IGameComponents _components;
        private IComponentsStorage _componentsStorage;
        private CinemachineCameraConfig _config;

        List<IDisposable> _disposables = new();

        private float _verticalRecoil;
        private float _recoilDuration;


        protected override void Awake(IGameComponents components)
        {

            _components = components;
            _componentsStorage = _components.BaseObject.GetComponent<IComponentsStorage>();
            _config = _componentsStorage.CinemachineCameraConfig;
            
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
            if (_recoilDuration > 0)
            {
                float recoilModifier = Input.GetMouseButton(1) ?  0.3f : 0.7f;
                _config.Y_Axis.Value -= (_verticalRecoil/ 10 * Time.deltaTime)/_recoilDuration * recoilModifier;
                _recoilDuration -= Time.deltaTime;
            }
            else
            {
                
            }

        }


        protected override void FixedUpdate()
        {

            _componentsStorage.CinemachineCameraConfig.Y_Axis.Update(Time.fixedDeltaTime);
            _componentsStorage.CinemachineCameraConfig.X_Axis.Update(Time.fixedDeltaTime);

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