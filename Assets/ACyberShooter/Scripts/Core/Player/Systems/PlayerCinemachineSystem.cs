using UnityEngine;
using Abstracts;
using EventBus;
using System;
using System.Collections.Generic;
using UniRx;
using Cinemachine;


namespace Core
{

    public class PlayerCinemachineSystem : BaseSystem, IDisposable
    {

        private IGameComponents _components;
        private IComponentsStorage _componentsStorage;
        private CinemachineFreeLook _freeLookCamera;

        List<IDisposable> _disposables = new();

        private float _verticalRecoil;
        private float _recoilDuration;


        protected override void Awake(IGameComponents components)
        {

            _components = components;
            _componentsStorage = _components.BaseObject.GetComponent<IComponentsStorage>();
            _freeLookCamera = _componentsStorage.FreeLookCamera;
            
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
                _freeLookCamera.m_YAxis.Value -= (_verticalRecoil/ 1000 * Time.deltaTime)/_recoilDuration;
                _recoilDuration -= Time.deltaTime;
            }
            else
            {
                
            }

        }

         
        public void Dispose()
        => PlayerEvents.OnGamePaused -= onPausedGame;
        
    }
}