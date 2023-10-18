using Cinemachine;
using Configs;
using Core;
using UniRx;
using UnityEngine;


namespace Abstracts
{

    public interface IComponentsStorage
    {

        IAnimatorIK AnimatorIK { get; }
        ReactiveCommand<WeaponRecoilConfig> RecoilCommand { get; set; }
        WeaponData WeaponData { get; }

        CameraConfig CameraConfig { get; }
        Transform CameraLookAt { get; set; }
        CinemachineCameraConfig CinemachineCameraConfig { get; set; }
        CrossHairTarget CrossHairTarget { get; set; }
        JumpConfig JumpConfig { get;}
        Transform GroundCheckerR { get; }
        Transform GroundCheckerL { get; }
        IInput Input { get; }
        void InitComponents();



    }
}