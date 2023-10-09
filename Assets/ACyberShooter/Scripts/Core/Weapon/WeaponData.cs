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
        
        [SerializeField] private LayerMask _ignoreRaycastLayerMask;


        public void InitData()
        {

            Weapons.ForEach(weapon => weapon.Muzzle.InitPool(_projectilesPool, _hitEffectsRoot, _ignoreRaycastLayerMask));
        }
    }


    
}