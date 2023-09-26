using System.Collections.Generic;
using Core;
using UnityEngine;


namespace Abstracts
{
    
    public abstract class StateMachine : MonoBehaviour
    {
        
        private List<ISystem> _systems;

        protected abstract List<ISystem> GetSystems();


        private void Awake()
        {
            _systems = GetSystems();

            ObjectStack stack = new ObjectStack(Camera.main, this.gameObject);

            for (int i = 0; i < _systems.Count; i++)
            {
                _systems[i].BaseAwake(stack);
            }
        }
        
        
        private void OnEnable()
        {
            for (int i = 0; i < _systems.Count; i++)
            {
                _systems[i].BaseOnEnable();
            }
        }
        
        
        private void Start()
        {
            for (int i = 0; i < _systems.Count; i++)
            {
                _systems[i].BaseStart();
            }
        }
        
        
        private void Update()
        {
            for (int i = 0; i < _systems.Count; i++)
            {
                _systems[i].BaseUpdate();
            }
        }
        
        
        private void FixedUpdate()
        {
            for (int i = 0; i < _systems.Count; i++)
            {
                _systems[i].BaseFixedUpdate();
            }
        }
        
        
        private void LateUpdate()
        {
            for (int i = 0; i < _systems.Count; i++)
            {
                _systems[i].BaseLateUpdate();
            }
        }


       
        
        
    }
}
