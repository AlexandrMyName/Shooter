using System.Collections.Generic;
using UnityEngine;


namespace Core
{
 
    public class WeaponData : MonoBehaviour
    {

        public List<Weapon> Weapons = new List<Weapon>();

        [SerializeField] private Transform _playerRoot;

        [SerializeField] private ParticleSystem _hitEffect;// Not correct (look at Weapon class)
        [SerializeField] private Transform _crossHairTransform;
        [SerializeField] private LayerMask _ignoreRaycastLayerMask;


        public void InitData()
        {

            Weapons.ForEach(weapon => weapon
                .Muzzle
                .InitPool(
                    _ignoreRaycastLayerMask,
                    weapon.MuzzleFlash, 
                    _crossHairTransform,
                    _hitEffect));
        }
    }


    
}