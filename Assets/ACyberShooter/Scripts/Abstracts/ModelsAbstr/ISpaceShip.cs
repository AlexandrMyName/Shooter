using Cinemachine;
using Configs;
using UnityEngine;


namespace Abstracts
{

    public interface ISpaceShip
    {

        Transform VisualModel { get; set; }
        CinemachineFreeLook Camera { get; set; }
        bool IsLockControll { get; set; }
        PlayerInput PlayerInput { get; set; }
        Rigidbody Rigidbody { get; set; }
        SpaceshipData SpaceshipData { get; set; }

        CinemachineCameraConfig CameraConfig { get; set;}
        GameObject DefaultProjectTile { get; set; }
        Transform ProjectTileSpawnTransform { get; set; }
        float DefaultProjectTileSpeed { get; set; }
        Transform GetTransform();

        void GetParametrs();



    }
}