using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Core 
{
     
    public class CrossHairTarget : MonoBehaviour
    {

        [SerializeField] private float _minDistance = 3f;
        [SerializeField] private float _lerpMultiplier = 3f;
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
            if (_targetPosition != null)
            {
                if(Vector3.Distance(_targetPosition,ray.origin ) > _minDistance)
                     transform.position = Vector3.Lerp(transform.position, _targetPosition, Time.deltaTime * _lerpMultiplier);

            }

        }
    }
}