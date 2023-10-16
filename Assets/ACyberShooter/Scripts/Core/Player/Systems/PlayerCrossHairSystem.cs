using UnityEngine;
using Abstracts;
using System;
using System.Collections.Generic;


namespace Core
{

    public class PlayerCrossHairSystem : BaseSystem, IDisposable
    {

        private IGameComponents _components;
        private IComponentsStorage _componentsStorage;

        private const float camera_offset_UP = 1.6f;//������ ������
        private Quaternion rotation;

        private Vector2 _inputMouseDirection = Vector2.zero;

        private Transform _crossHairParent;

        List<IDisposable> _disposables = new();


        protected override void Awake(IGameComponents components)
        {

            _components = components;
            _componentsStorage = _components.BaseObject.GetComponent<IComponentsStorage>();
            var crossHairParent = GameObject.Instantiate(new GameObject("CrossHairParent")/*, _components.BaseObject.transform*/);
            _crossHairParent = crossHairParent.transform;
            _componentsStorage.CrossHairTarget.gameObject.transform.parent = _crossHairParent;
            _crossHairParent.position += Vector3.up * camera_offset_UP;
              
        }
         

        protected override void Update()
        {

            _inputMouseDirection.x = Input.GetAxisRaw("Mouse X");
            _inputMouseDirection.y = Input.GetAxisRaw("Mouse Y");
            _crossHairParent.transform.position = _components.BaseObject.transform.position + Vector3.up * camera_offset_UP;
            
        }


        protected override void FixedUpdate()
        {
            
            rotation.x += -_inputMouseDirection.y * _componentsStorage.CameraConfig.CameraSpeedMultiplier;
            rotation.y += _inputMouseDirection.x * _componentsStorage.CameraConfig.CameraSpeedMultiplier;

            rotation.x = Mathf.Clamp(rotation.x, _componentsStorage.CameraConfig.CameraClampMin, _componentsStorage.CameraConfig.CameraClampMax);
            rotation.z = 0;
             
            Quaternion newRot = Quaternion.Euler(rotation.x, rotation.y, rotation.z);
             
                _crossHairParent.rotation 
                    = Quaternion.SlerpUnclamped(_crossHairParent.rotation, newRot,
                        Time.deltaTime * _componentsStorage.CameraConfig.CameraSpeedMultiplier);
                
        }

        public void Dispose() { } 
        
    }
}