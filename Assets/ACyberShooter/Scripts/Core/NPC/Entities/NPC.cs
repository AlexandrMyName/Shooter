using Abstracts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Core
{

    public class NPC : StateMachine
    {


        protected override List<ISystem> GetSystems()
        {
            return new List<ISystem>()
            {
                new AI_FriendlySystem(),
            };
        }
    }
}