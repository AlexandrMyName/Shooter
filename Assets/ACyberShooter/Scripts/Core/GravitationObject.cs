using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core;
using Abstracts;

public class GravitationObject : MonoBehaviour
{

    [SerializeField] private Transform _attractionedObject;
    [SerializeField] private SpaseShip _spaceShip;
    [SerializeField] private Transform _centerGravityTransform;
    [SerializeField] private List<GameObject> _hidebleObjectsOnMaxDistance = new();
    [SerializeField] private List<GameObject> _shownableObjectsOnMaxDistance = new();
    [SerializeField] private float _gravityForce = 0.03f;
    [SerializeField] private float _shipRotationDrag = .5f;

    [SerializeField] private float _maxGravityForce = .39f;

    [SerializeField] private List<Transform> _centerGravityTransforms = new();

    private int _currentGravityCenterPointIndex;

     
    private void Update()
    {

        if (_spaceShip.Component.Health <= 0) return;

        _attractionedObject.position = Vector3.Lerp(_attractionedObject.position, _centerGravityTransform.position,
            Time.deltaTime * _gravityForce * (Vector3.Distance(_attractionedObject.position,_centerGravityTransform.position)/1000));
        _attractionedObject.rotation
            = Quaternion.Slerp(_attractionedObject.rotation, 
            Quaternion.LookRotation(_centerGravityTransform.position, Vector3.up),
            Time.deltaTime * _shipRotationDrag * (Vector3.Distance(_attractionedObject.position, _centerGravityTransform.position)/1000));

        if(Vector3.Distance(_attractionedObject.position, _centerGravityTransform.position) < 700f)
        {
            _gravityForce = _maxGravityForce;
            _shipRotationDrag = _maxGravityForce;
            _spaceShip.enabled = false;

            foreach(var hiden in _hidebleObjectsOnMaxDistance)
            {
                hiden.SetActive(false);
            }
            foreach (var shown in _shownableObjectsOnMaxDistance)
            {
                shown.SetActive(true);
            }
        }

        if(Vector3.Distance(_attractionedObject.position, _centerGravityTransform.position) < 100f)
        {
            
            _currentGravityCenterPointIndex++;
            if (_centerGravityTransforms.Count - 1 >= _currentGravityCenterPointIndex)
                _centerGravityTransform = _centerGravityTransforms[_currentGravityCenterPointIndex];
        }
    }
}
