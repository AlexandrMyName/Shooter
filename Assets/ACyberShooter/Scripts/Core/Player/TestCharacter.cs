using System.Collections.Generic;
using Abstracts;
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
                 new PlayerCameraSystem(),
                 new PlayerCrossHairSystem(),
                 new PlayerMovableSystem(),
                 new PlayerShootSystem(),
                 new PlayerJumpSystem()
             };
        }
    }
}