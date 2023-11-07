using Abstracts;
using Cinemachine;
using Configs;
using FischlWorks;
using UniRx;
using UnityEngine;


namespace Core
{

    public class ComponentsStorage : MonoBehaviour, IComponentsStorage
    {

        [SerializeField] private AnimatorIK _animatorIK;

        [field:SerializeField] public WeaponData WeaponData { get; private set; }
        public ReactiveProperty<Vector3> Recoil { get; set; }
        [field: SerializeField] public CrossHairTarget CrossHairTarget { get; set; }
        [field: SerializeField] public CameraConfig CameraConfig { get; set; }
        [field: SerializeField] public CinemachineCameraConfig CinemachineCameraConfig { get; set; }
        [field: SerializeField] public Transform CameraLookAt { get; set; }
        public IAnimatorIK AnimatorIK {get; private set;}
        [field: SerializeField] public JumpConfig JumpConfig { get; private set; }
        [field: SerializeField] public LocomotionConfig LocomotionConfig { get; set; }
        [field: SerializeField] public Transform GroundCheckerR { get; private set; }
        [field: SerializeField] public Transform GroundCheckerL { get; private set; }
        [field: SerializeField] public csHomebrewIK Foot_IK { get;  set; }

        public ReactiveCommand<WeaponRecoilConfig> RecoilCommand { get; set; }

        public IInput Input { get; private set; }


        public void InitComponents()
        {

            RecoilCommand = new();
           // Recoil.SkipLatestValueOnSubscribe();
           
            AnimatorIK = _animatorIK;
            Input = NewInput.Instance;
        }

        
    }
}