using System.Collections.Generic;
using Abstracts;
using Cinemachine;
using UniRx.Triggers;
using UnityEngine;


namespace Core
{

    [RequireComponent(typeof(ComponentsStorage))]
    public class TestCharacter : StateMachine, IPlayer
    {

        public IComponentsStorage ComponentsStorage { get; private set; }


        protected override List<ISystem> GetSystems()
        {

            ComponentsStorage = GetComponent<ComponentsStorage>();
            ComponentsStorage.InitComponents();

             return new List<ISystem>()
             {
                 
                 new PlayerCrossHairSystem(),
                 new PlayerLocomotionSystem(),
                 new PlayerShootSystem(),
                 new PlayerJumpSystem()
             };
        }
    }
}