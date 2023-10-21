using Abstracts;
using Cinemachine;
using Configs;
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
        [field: SerializeField] public CinemachineFreeLook FreeLookCamera { get; set; }
        public IAnimatorIK AnimatorIK {get; private set;}
        [field: SerializeField] public JumpConfig JumpConfig { get; private set; }
        [field: SerializeField] public Transform GroundCheckerR { get; private set; }
        [field: SerializeField] public Transform GroundCheckerL { get; private set; }
        public ReactiveCommand<WeaponRecoilConfig> RecoilCommand { get; set; }

        public void InitComponents()
        {

            RecoilCommand = new();
           // Recoil.SkipLatestValueOnSubscribe();
         
            AnimatorIK = _animatorIK;

        }

        
    }
}