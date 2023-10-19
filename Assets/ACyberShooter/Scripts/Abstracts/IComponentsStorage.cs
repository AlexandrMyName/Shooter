using Configs;
using Core;
using UniRx;
using UnityEngine;

namespace Abstracts
{

    public interface IComponentsStorage
    {

        IAnimatorIK AnimatorIK { get; }
        ReactiveProperty<Vector3> Recoil { get; set; }
        WeaponData WeaponData { get; }
        CameraConfig CameraConfig { get; }
        CrossHairTarget CrossHairTarget { get; set; }
        void InitComponents();



    }
}