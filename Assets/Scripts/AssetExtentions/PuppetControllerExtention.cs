using EventBus;
using RootMotion.Demos;
using UnityEngine;

public class PuppetControllerExtention : MonoBehaviour
{
    [SerializeField] private CharacterPuppet _characterController;

    private void OnEnable()
    {
        ShootingEvents.OnCameraDirectionRotate += ChangeRotationInCameraDirection;
    }

    private void OnDisable()
    {
        ShootingEvents.OnCameraDirectionRotate -= ChangeRotationInCameraDirection;
    }

    private void ChangeRotationInCameraDirection(bool isRotating)
    {
        _characterController.lookInCameraDirection = isRotating;
    }
}
