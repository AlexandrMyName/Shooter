using Abstracts;
using Configs;
using UnityEngine;


namespace Core
{

    public class ComponentsStorage : MonoBehaviour, IComponentsStorage
    {

        [SerializeField] private AnimatorIK _animatorIK;

        [field: SerializeField] public CameraConfig CameraConfig { get; set; }
        public IAnimatorIK AnimatorIK {get; private set;}
        

        public void InitComponents()
        {

            AnimatorIK = _animatorIK;

        }

    }
}