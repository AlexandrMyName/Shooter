using Abstracts;
using Configs;
using Enums;
using EventBus;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;


namespace Core
{

    [Serializable]
    public class Weapon
    {
  
        public GameObject WeaponObject;
        public IWeaponType Type; // oops, this not interface)
       
        [Range(0,1f)] public float RigDuration;
        public Muzzle Muzzle;
        public ParticleSystem[] MuzzleFlash;
        public bool IsActive { get;set;}
    }
}