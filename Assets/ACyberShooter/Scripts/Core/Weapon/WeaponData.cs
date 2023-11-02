using Abstracts;
using System.Collections.Generic;
using System.Linq;

#if UNITY_EDITOR
using UnityEditor.Animations;
#endif

using UnityEngine;
using UnityEngine.UIElements;
using Views;


namespace Core
{

    [RequireComponent(typeof(Animator))]
    public class WeaponData : MonoBehaviour
    {

        [HideInInspector] public List<Weapon> Weapons = new();
        
        [field:SerializeField, Space] public WeaponModel_View PrimaryAutoWeaponViewsFab { get; set; }
        [SerializeField] private Transform _primaryRoot;
        [field: SerializeField, Space] public WeaponModel_View SecondaryPistolWeaponViewsFab { get; set; }
        [SerializeField] private Transform _secondaryRoot;

        [field: SerializeField, Space] public WeaponModel_View RocketLauncherWeaponViewsFab { get; set; }
        [SerializeField] public Transform _rocketLauncherWeaponViewsFabRoot;

        [Space(20)]

        [SerializeField] private Transform _playerRoot;
        [SerializeField] private Transform _weaponsRoot;

  
  
        [SerializeField] private Transform _crossHairTransform;
        [SerializeField] private LayerMask _ignoreRaycastLayerMask;

        [HideInInspector] public int PrimaryIndex = 0;
        [HideInInspector] public int SecondaryIndex = 1;

        [Space,SerializeField] private ComponentsStorage _playerComponents;
         
        public Weapon CurrentWeapon { get; set; }


        public void InitData()
        {

            if(PrimaryAutoWeaponViewsFab != null)
                AddWeapon(PrimaryAutoWeaponViewsFab, _primaryRoot);
             
            if (SecondaryPistolWeaponViewsFab != null)
                AddWeapon(SecondaryPistolWeaponViewsFab, _secondaryRoot);
                 
            if(RocketLauncherWeaponViewsFab != null)
                AddWeapon(RocketLauncherWeaponViewsFab, _rocketLauncherWeaponViewsFabRoot);
  

            if (Weapons.Count > 0)
            {
                
                CurrentWeapon = Weapons.First();
                CurrentWeapon.IsActive = false;
            }

        }


        public void AddWeapon(WeaponModel_View weaponView, Transform weaponSlot, bool isActive = true)
        {

            if (weaponSlot == null) weaponSlot = _weaponsRoot;

            Weapon addedWeapon = weaponView.GetWeapon();
            Weapon findedWeapon = Weapons.Find(weap => weap.Type == addedWeapon.Type);

            if (findedWeapon == null)
            {

                var viewInstance = GameObject.Instantiate(weaponView, weaponSlot);


                Weapons.Add(viewInstance.GetWeapon());

                Weapon newWeapon = viewInstance.GetWeapon();
                newWeapon.RecoilCommand = _playerComponents.RecoilCommand;
                newWeapon.Muzzle.InitPool(_ignoreRaycastLayerMask, newWeapon.MuzzleFlash, _crossHairTransform, newWeapon.RecoilConfig,_playerComponents);
                  
                newWeapon.WeaponObject.SetActive(isActive);
                 
            }
            else
            {
                //Already - weapon can be refreshed
            }
        }

 
        public void RemoveWeapon(IWeaponType weaponType)
        {

        }
    }
}