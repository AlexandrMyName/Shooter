using System;
using RootMotion.Dynamics;
using UnityEngine;


namespace Abstracts
{

    public interface IAnimatorIK : IDisposable
    {

        void SetLayerWeight(int indexLayer, float weight);
       
        void SetFloat(string keyID, float value);
        void SetFloat(string keyID, float value, float delta);

        void SetBool(string keyID, bool value);
        void SetWeaponState(IWeaponType weaponType);

        GameObject PuppetObject { get; }
        public PuppetMaster PuppetMaster { get; }

    }
}