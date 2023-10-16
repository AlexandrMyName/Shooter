using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Core 
{
     
    public class CrossHairTarget : MonoBehaviour
    {

        private Camera _camera;
        Ray ray;
        RaycastHit hit;


        private void Awake() => _camera = Camera.main;
         

        private void Update()
        {

            ray.origin = _camera.transform.position;
            ray.direction = _camera.transform.forward;

            if(Physics.Raycast(ray, out hit))
            {

                transform.position = hit.point;
            }
        }
    }
}