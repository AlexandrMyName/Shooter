using Abstracts;
using System;
using UnityEngine;


namespace Core
{

    [Serializable]
    public class Weapon
    {

        public IWeaponType Type; // oops, this not interface)
        public Transform rightHandHint;
        public Transform leftHandHint;
        //Add configs to this
    }
}