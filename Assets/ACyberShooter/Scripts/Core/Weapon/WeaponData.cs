using Abstracts;
using System;
using System.Collections.Generic;
using UnityEngine;


namespace Core
{
 
    public class WeaponData : MonoBehaviour
    {

        public List<Weapon> Weapons = new List<Weapon>();

    }


    [Serializable]
    public class Weapon
    {

        public IWeaponType Type; // oops, this not interface)
        public Transform rightHandHint;
        public Transform leftHandHint;
        //Add configs to this
    }
}