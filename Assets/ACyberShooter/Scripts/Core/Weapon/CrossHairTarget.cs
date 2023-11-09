using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Core 
{
     
    public class CrossHairTarget : MonoBehaviour
    {

        [SerializeField] private Transform _rootTransform;
        [SerializeField] private Transform _defaultTarget;
        [SerializeField] private float _maxTargetAngle = 30f;

        private Camera _camera;
        Ray ray;
        RaycastHit hit;

        Vector3 _targetPosition;

        private void Awake() => _camera = Camera.main;
         

        private void Update()
        {

            ray.origin = _camera.transform.position;
            ray.direction = _camera.transform.forward;

            if(Physics.Raycast(ray, out hit))
            {
                _targetPosition =  hit.point;
            }
            else
            {
                transform.position = _defaultTarget.position;
                return;
            }


            if (_targetPosition != null)
            {
                var angle = Vector3.Angle(_rootTransform.forward, (hit.point - _rootTransform.transform.position).normalized);

                if (angle < _maxTargetAngle)
                    transform.position = Vector3.Lerp(transform.position, _targetPosition, Time.deltaTime * _maxTargetAngle);
                else transform.position = _defaultTarget.position;
            }

        }
    }
}