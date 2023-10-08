using Abstracts;
using System;
using System.Collections.Generic;
using UnityEngine;


namespace Core
{
 
    public class WeaponData : MonoBehaviour
    {

        public List<Weapon> Weapons = new List<Weapon>();

        [SerializeField] private Transform _playerRoot;
        [SerializeField] private Transform _projectilesPool;
        [SerializeField] private Transform _hitEffectsRoot;

        [SerializeField] private int _playerLayerIndex = 8;
        [SerializeField] private int _playerRagdollLayerIndex = 9;


        public void InitData()
        {

            Weapons.ForEach(weapon => weapon.Muzzle.InitPool(_projectilesPool, _hitEffectsRoot, _playerRagdollLayerIndex, _playerLayerIndex));
        }
    }


    
}