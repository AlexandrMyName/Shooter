using Configs;
using Core;


namespace Abstracts
{

    public interface IComponentsStorage
    {

        IAnimatorIK AnimatorIK { get; }
        CameraConfig CameraConfig { get; }
        CrossHairTarget CrossHairTarget { get; set; }
        void InitComponents();



    }
}