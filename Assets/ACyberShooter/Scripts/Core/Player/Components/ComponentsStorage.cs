using Abstracts;
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
        public IAnimatorIK AnimatorIK {get; private set;}
        [field: SerializeField] public JumpConfig JumpConfig { get; private set; }
        [field: SerializeField] public Transform GroundChecker { get; private set; }
        [field: SerializeField] public Transform GroundCheckerR { get; private set; }
        [field: SerializeField] public Transform GroundCheckerL { get; private set; }

        public void InitComponents()
        {

            Recoil = new(Vector3.zero);
           // Recoil.SkipLatestValueOnSubscribe();
         
            AnimatorIK = _animatorIK;

        }

        
    }
}