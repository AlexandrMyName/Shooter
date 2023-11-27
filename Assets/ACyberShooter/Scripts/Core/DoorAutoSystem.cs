using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Core
{

    public class DoorAutoSystem : MonoBehaviour
    {

        public Transform _leftDoor;
        public Transform _rightDoor;

        public bool _canOpen;
        public float _speedDoors;
        [Header("Left Door"), Space(20)]
        public Vector3 _inOpenedPosition_leftDoor;
        public Vector3 _inClosedPosition_leftDoor;
        [Header("Right Door"), Space(20)]
        public Vector3 _inOpenedPosition_rightDoor;
        public Vector3 _inClosedPosition_rightDoor;

        private bool _isOpened;


        private void OnTriggerEnter(Collider other)
        {

            if (other.tag == "Player")
            {
                _isOpened = true;
            }
        }


        private void OnTriggerExit(Collider other)
        {

            if (other.tag == "Player")
            {
                _isOpened = false;
            }
        }


        private void Update()
        {

            if (!_canOpen) return;

            if (_isOpened)
            {
                if (_leftDoor != null)
                    _leftDoor.localPosition = Vector3.Lerp(_leftDoor.localPosition, _inOpenedPosition_leftDoor, Time.deltaTime * _speedDoors);

                if (_rightDoor != null)
                    _rightDoor.localPosition = Vector3.Lerp(_rightDoor.localPosition, _inOpenedPosition_rightDoor, Time.deltaTime * _speedDoors);
            }
            else
            {
                if (_leftDoor != null)
                    _leftDoor.localPosition = Vector3.Lerp(_leftDoor.localPosition, _inClosedPosition_leftDoor, Time.deltaTime * _speedDoors);

                if (_rightDoor != null)
                    _rightDoor.localPosition = Vector3.Lerp(_rightDoor.localPosition, _inClosedPosition_rightDoor, Time.deltaTime * _speedDoors);
            }
        }
    }
}