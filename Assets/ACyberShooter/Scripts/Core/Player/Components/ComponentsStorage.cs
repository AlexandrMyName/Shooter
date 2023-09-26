using Abstracts;
using UnityEngine;


namespace Core
{

    public class ComponentsStorage : MonoBehaviour, IComponentsStorage
    {

        [SerializeField] private AnimatorIK _animatorIK;

        public IAnimatorIK AnimatorIK {get; private set;}


        private void Awake()
        {

            AnimatorIK = _animatorIK;

        }

    }
}