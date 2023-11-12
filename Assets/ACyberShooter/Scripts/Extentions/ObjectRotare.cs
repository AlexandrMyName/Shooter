using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ObjectRotare : MonoBehaviour
{

    [SerializeField] private List<ObjectRotation> _rotationObjects = new();


    private void Update()
    {

        _rotationObjects.ForEach(obj =>
        {
            obj.RotationObject.Rotate(obj.Direction, obj.Speed * Time.deltaTime);
        });
    }


    [Serializable]
    class ObjectRotation
    {

        public Transform RotationObject;
        public Vector3 Direction;
        public float Speed;
    }
}


