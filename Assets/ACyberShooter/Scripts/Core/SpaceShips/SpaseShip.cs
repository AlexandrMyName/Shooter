using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Abstracts;

namespace Core
{
    public class SpaseShip : StateMachine
    {

        [field:SerializeField] public SpaceShipComponent Component { get; private set; }


        protected override List<ISystem> GetSystems()
        {
            
            return new List<ISystem>()
            {
                new SpaceShipCameraSystem(),
                new SpaceShipMovableSystem(),
                new SpaceShipHPSystem(),
            };
        }
    }
}