using System;
using RootMotion.Dynamics;
using UnityEngine;


namespace Abstracts
{

    public interface IAnimatorIK : IDisposable
    {

        bool IsLoseBalance { get; set; }
        float Y_Velocity { get;set; }
        void SetLayerWeight(int indexLayer, float weight);
       
        void SetFloat(string keyID, float value);
        void SetFloat(string keyID, float value, float delta);

        void SetBool(string keyID, bool value);
        void SetWeaponState(IWeaponType weaponType, bool useHolster = true);
        void SetRootMotion(Vector3 targetDirection, Quaternion targetRotation);
        GameObject PuppetObject { get; }
        PuppetMaster PuppetMaster { get; }

        Animator Animator { get; }

    }
}