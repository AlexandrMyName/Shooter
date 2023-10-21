using UnityEngine;
using Abstracts;
using EventBus;
using System;
using System.Collections.Generic;
using UniRx;
using Random = UnityEngine.Random;

namespace Core
{

    public class PlayerCameraSystem : BaseSystem, IDisposable
    {

        private IGameComponents _components;
        private IComponentsStorage _componentsStorage;

        private const float camera_offset_UP = 1.6f;//������ ������
        private Quaternion rotation;

        private Vector2 _inputMouseDirection = Vector2.zero;

        private Transform _cameraParent;

        List<IDisposable> _disposables = new();
         
        private float _recoil;


        protected override void Awake(IGameComponents components)
        {

            _components = components;
            _componentsStorage = _components.BaseObject.GetComponent<IComponentsStorage>();
            var cameraParent = GameObject.Instantiate(new GameObject("CameraParent")/*, _components.BaseObject.transform*/);
            _cameraParent = cameraParent.transform;
            _components.MainCamera.transform.parent = _cameraParent;
            
            _cameraParent.position += Vector3.up * camera_offset_UP;
             
            PlayerEvents.OnGamePaused += onPausedGame;
             
            _componentsStorage.Recoil.Subscribe (value =>
            {
                 
                if (value != Vector3.zero)
                    _recoil = value.y;
               
                 
            }).AddTo(_disposables);
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

                if(Input.GetMouseButtonUp(0))
                {
                    _recoil = 0;
                }
            if (_recoil > 0 )
            {
                var Max_y_recoil = Quaternion.Euler(Vector3.left * _recoil);

                _components.MainCamera.transform.localRotation 
                    = Quaternion.Slerp(_components.MainCamera.transform.localRotation, Max_y_recoil, Time.deltaTime * 1000);
                _recoil -= Time.deltaTime;

            }
            else
            {
                _componentsStorage.Recoil.Value = Vector3.zero;
                var ZeroAngle = Quaternion.Euler(0, 0, 0);
                _recoil = 0f;
                _components.MainCamera.transform.localRotation
                    = Quaternion.Slerp(_components.MainCamera.transform.localRotation, ZeroAngle,  Time.deltaTime * 1000);

            }
         
            _inputMouseDirection.x = Input.GetAxis("Mouse X");
            _inputMouseDirection.y = Input.GetAxis("Mouse Y");
            _cameraParent.transform.position = _components.BaseObject.transform.position + Vector3.up * camera_offset_UP;
            
        }


        protected override void LateUpdate()
        {

            RaycastHit hit;
            Ray ray = new Ray(_cameraParent.position,
            _components.MainCamera.transform.position - _cameraParent.position
            );

            if (Physics.Raycast(ray,
                out hit,
               Mathf.Abs(_componentsStorage.CameraConfig.CameraOffSet_Normal.z), _componentsStorage.CameraConfig.ObstacleLayer))
            {
                _components.MainCamera.transform.position = hit.point;
            }
            else _components.MainCamera.transform.position =
                 _cameraParent
                     .TransformPoint(_componentsStorage.CameraConfig.CameraOffSet_Normal);
             


            if (Vector3.Distance(
                _cameraParent.position,
                _components.MainCamera.transform.position) < _componentsStorage.CameraConfig.MinDistance)
                _components.MainCamera.cullingMask = _componentsStorage.CameraConfig.NoPlayerLayer;

            else _components.MainCamera.cullingMask = _componentsStorage.CameraConfig.WithPlayerLayer;

            rotation.x += -_inputMouseDirection.y * _componentsStorage.CameraConfig.CameraSpeedMultiplier;
            rotation.y += _inputMouseDirection.x * _componentsStorage.CameraConfig.CameraSpeedMultiplier;

            rotation.x = Mathf.Clamp(rotation.x, _componentsStorage.CameraConfig.CameraClampMin, _componentsStorage.CameraConfig.CameraClampMax);
            rotation.z = 0;
             

            Quaternion newRot = Quaternion.Euler(rotation.x, rotation.y, rotation.z);

            _cameraParent.transform.rotation = newRot;
          
        }

        public void Dispose()
        => PlayerEvents.OnGamePaused -= onPausedGame;
        
    }
}