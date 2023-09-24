using EventBus;
using RootMotion.Demos;
using UnityEngine;

namespace AssetExtentions
{
    public class PuppetControllerExtention : MonoBehaviour
    {
        [SerializeField] private CharacterPuppet _characterController;
        [SerializeField] private Player.CameraController _cameraController;

        private void Start()
        {
            ShootingEvents.OnCameraDirectionRotate += ChangeRotationInCameraDirection;
            PlayerEvents.OnGamePaused += ChangeCursorLockState;
        }

        private void OnDisable()
        {
            ShootingEvents.OnCameraDirectionRotate -= ChangeRotationInCameraDirection;
            PlayerEvents.OnGamePaused -= ChangeCursorLockState;
        }

        private void ChangeRotationInCameraDirection(bool isRotating)
        {
            _characterController.lookInCameraDirection = isRotating;
        }

        private void ChangeCursorLockState(bool isLocked)
        {
            _cameraController.lockCursor = !isLocked;
        }
    }
}
