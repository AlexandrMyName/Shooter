using Cinemachine;
using Configs;
using MVC.Core;
using UnityEngine;
using UnityEngine.Rendering;

namespace Abstracts
{

    public interface ISpaceShip
    {

        Transform VisualModel { get; set; }
        CinemachineFreeLook Camera { get; set; }
        CinemachineFreeLook CameraWithShake { get; set; }
        bool IsLockControll { get; set; }
        PlayerInput PlayerInput { get; set; }
        Rigidbody Rigidbody { get; set; }
        SpaceshipData SpaceshipData { get; set; }

        CinemachineCameraConfig CameraConfig { get; set;}
        GameObject DefaultProjectTile { get; set; }
        Transform ProjectTileSpawnTransform { get; set; }
        float DefaultProjectTileSpeed { get; set; }
        float Health { get; set; }

        Volume DeathPostProccessVolume { get; set; }
        float VisualDeathVolumeSpeed { get; set; }
        int CurrentSceneIndex { get; set; }
        GameMonoBeh GameUIBehavior { get; set; }
        Transform GetTransform();

        void GetParametrs();



    }
}