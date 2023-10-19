using Configs;
using Core;


namespace Abstracts
{

    public interface IComponentsStorage
    {

        IAnimatorIK AnimatorIK { get; }
        WeaponData WeaponData { get; }
        CameraConfig CameraConfig { get; }
        CrossHairTarget CrossHairTarget { get; set; }
        void InitComponents();



    }
}