using Configs;


namespace Abstracts
{

    public interface IComponentsStorage
    {

        IAnimatorIK AnimatorIK { get; }
        CameraConfig CameraConfig { get; }

        void InitComponents();



    }
}