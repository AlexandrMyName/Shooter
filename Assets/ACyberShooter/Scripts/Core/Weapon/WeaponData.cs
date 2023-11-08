using Abstracts;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
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

        [field: SerializeField, Space] public WeaponModel_View SwordWeaponViewsFab { get; set; }
        [SerializeField] public Transform _swordWeaponViewsFabRoot;

        [field: SerializeField, Space] public WeaponModel_View NoneWeaponViewsFab { get; set; }

        [Space(20)]
        [SerializeField] private IWeaponType _defaultWeapon = IWeaponType.Pistol;
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

            AddWeapons();

            if (Weapons.Count > 0)
            {

                CurrentWeapon = Weapons.First();
                CurrentWeapon.IsActive = false;
            }

        }

        private void AddWeapons()
        {

            Dictionary<WeaponModel_View, Transform> weaponsModelViews = new();

            if (PrimaryAutoWeaponViewsFab != null) weaponsModelViews.Add(PrimaryAutoWeaponViewsFab, _primaryRoot);
            if (SecondaryPistolWeaponViewsFab != null) weaponsModelViews.Add(SecondaryPistolWeaponViewsFab , _secondaryRoot);
            if (RocketLauncherWeaponViewsFab != null) weaponsModelViews.Add(RocketLauncherWeaponViewsFab, _rocketLauncherWeaponViewsFabRoot);
            
            foreach(var weapon in weaponsModelViews)
            {
                if (weapon.Key.GetWeapon().Type == _defaultWeapon)
                    AddWeapon(weapon.Key, weapon.Value);
            }

            if (PrimaryAutoWeaponViewsFab != null && PrimaryAutoWeaponViewsFab.GetWeapon().Type != _defaultWeapon)
                AddWeapon(PrimaryAutoWeaponViewsFab, _primaryRoot);

            if (SecondaryPistolWeaponViewsFab != null && SecondaryPistolWeaponViewsFab.GetWeapon().Type != _defaultWeapon)
                AddWeapon(SecondaryPistolWeaponViewsFab, _secondaryRoot);

            if (RocketLauncherWeaponViewsFab != null && RocketLauncherWeaponViewsFab.GetWeapon().Type != _defaultWeapon)
                AddWeapon(RocketLauncherWeaponViewsFab, _rocketLauncherWeaponViewsFabRoot);

            if (SwordWeaponViewsFab != null && SwordWeaponViewsFab.GetWeapon().Type != _defaultWeapon)
                AddWeapon(SwordWeaponViewsFab, _swordWeaponViewsFabRoot);

            if (NoneWeaponViewsFab != null)
            {
                AddWeapon(NoneWeaponViewsFab, _primaryRoot);
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