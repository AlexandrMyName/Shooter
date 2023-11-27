using System;
using System.Collections.Generic;
using Core;
using RootMotion.Dynamics;
using UnityEngine;


namespace Abstracts
{

    public interface IAnimatorIK : IDisposable
    {

        bool IsLoseBalance { get; set; }
        bool IsJump { get; set; }
        float Y_Velocity { get;set; }
        bool FpsCamera { get; set; }
        bool IsLocked { get; set; }
        void InitComponent(IComponentsStorage componentStorage, WeaponData weaponData);
        void SetLayerWeight(int indexLayer, float weight);
       
        void SetFloat(string keyID, float value);
        void SetFloat(string keyID, float value, float delta);

        void SetBool(string keyID, bool value);
        void SetWeaponState(IWeaponType weaponType, bool useHolster = true);
        void SetRootMotion(Vector3 targetDirection, Quaternion targetRotation);
        void InitDefaultWeapon(List<Weapon> weapons);
        void ChangeHeadObject(GameObject head);
        GameObject PuppetObject { get; }
        PuppetMaster PuppetMaster { get; }

        Animator Animator { get; }

    }
}