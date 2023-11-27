using Cinemachine;
using Configs;
using Core;
using FischlWorks;
using System.Collections.Generic;
using UniRx;
using UnityEngine;


namespace Abstracts
{

    public interface IComponentsStorage
    {

        IAnimatorIK AnimatorIK { get; }
        ReactiveCommand<WeaponRecoilConfig> RecoilCommand { get; set; }
        List<MeshRenderer> MeshRenderers { get; set; }
        WeaponData WeaponData { get; }
        WeaponInventory WeaponInventory { get; }
        csHomebrewIK Foot_IK { get; set; }
        CameraConfig CameraConfig { get; }
        Transform CameraLookAt { get; set; }
        CinemachineCameraConfig CinemachineCameraConfig { get; set; }
        LocomotionConfig LocomotionConfig { get; set; }
        CrossHairTarget CrossHairTarget { get; set; }
        JumpConfig JumpConfig { get;}
        IInput Input { get; }
        void InitComponents();



    }
}