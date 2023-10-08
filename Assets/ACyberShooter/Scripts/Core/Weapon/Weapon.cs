using Abstracts;
using Configs;
using Enums;
using EventBus;
using System;
using UnityEngine;
using UnityEngine.Animations.Rigging;


namespace Core
{

    [Serializable]
    public class Weapon
    {

        public GameObject weaponObject;
        public IWeaponType Type; // oops, this not interface)
        public TwoBoneIKConstraint rightHandIK_aiming;
        public TwoBoneIKConstraint leftHandIK_aiming;
 
        public TwoBoneIKConstraint rightHandIK_NoAiming;
        public TwoBoneIKConstraint leftHandIK_NoAiming;
        public Muzzle Muzzle;

    }
}