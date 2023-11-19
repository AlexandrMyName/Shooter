using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorSystem : MonoBehaviour
{

    [SerializeField] private Transform _doorTransform;
    [SerializeField] private float _speed;
    [SerializeField] private Vector3 _rotationClosesed;
    [SerializeField] private Vector3 _positionClosesed;

    [SerializeField] private bool _useRotation;
    bool _isCloseProccess;
    bool _isOpenProccess;


    [ContextMenu("Open")]
    public void Open()
    {
        _isOpenProccess = true;
        _isCloseProccess = false;
    }

    [ContextMenu("Close")]
    public void Close()
    {
        _isOpenProccess = false;
        _isCloseProccess = true;
    }


    private void Update()
    {

        if (_isCloseProccess)
        {
            if(_useRotation)
            _doorTransform.rotation = Quaternion.Slerp(_doorTransform.rotation, Quaternion.Euler(_rotationClosesed),Time.deltaTime * _speed );
            
        }
    }
}
