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

        [Header("SpaceShipSettings"),Space(20)]
        [SerializeField,Tooltip("Can be null")] 
        private bool _useSpaceShipOnly;
        [SerializeField, Tooltip("Can be null")]
        private Collider _defaultSpaceShipCollider;
        [Space(20), SerializeField] private string _characterName;
        
        public IComponentsStorage ComponentsStorage { get; private set; }


        protected override List<ISystem> GetSystems()
        {

            ComponentsStorage = GetComponent<ComponentsStorage>();
            ComponentsStorage.InitComponents();

             return new List<ISystem>()
             {
                 new PlayerInputSystem(),
                 new PlayerCinemachineSystem(),
                 new PlayerLocomotionSystem(),
                 new PlayerShootSystem(),
                 new PlayerJumpSystem(),
                 new PlayerSpaceShipControllSystem(_useSpaceShipOnly,_defaultSpaceShipCollider),
                 
             };
        }
    }
}