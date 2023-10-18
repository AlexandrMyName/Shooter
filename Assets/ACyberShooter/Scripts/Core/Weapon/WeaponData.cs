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
 
        [field:SerializeField] public WeaponModel_View PrimaryWeaponViewsFab { get; set; }
        [field: SerializeField] public WeaponModel_View SecondaryWeaponViewsFab { get; set; }

        [SerializeField] private Transform _primaryWeaponSlot;
        [SerializeField] private Transform _secondaryWeaponSlot;    

        [SerializeField] private Transform _playerRoot;
        [SerializeField] private Transform _weaponsRoot;

        [Header("Information of hands IK (Animation Rigging - Targets)")]
        [SerializeField] private Transform _leftHand_IK;
        [SerializeField] private Transform _rightHand_IK;

        [SerializeField] private ParticleSystem _hitEffect;// Not correct (look at Weapon class)

        [SerializeField] private Transform _crossHairTransform;
        [SerializeField] private LayerMask _ignoreRaycastLayerMask;

        public Weapon CurrentWeapon { get; set; }


        public void InitData()
        {
            AddWeapon(PrimaryWeaponViewsFab, true, _primaryWeaponSlot);
            AddWeapon(SecondaryWeaponViewsFab, false, _secondaryWeaponSlot);
             
            if (Weapons.Count > 0)
            {
                
                CurrentWeapon = Weapons.First();
                CurrentWeapon.WeaponObject.SetActive(true);
                CurrentWeapon.IsActive = false;
            }

        }


        public void AddWeapon(WeaponModel_View weaponView, bool isActivate, Transform weaponSlot)
        {
             
            Weapon addedWeapon = weaponView.GetWeapon();
            Weapon findedWeapon = Weapons.Find(weap => weap.Type == addedWeapon.Type);

            if (findedWeapon == null)
            {

                var viewInstance = GameObject.Instantiate(weaponView, weaponSlot);


                Weapons.Add(viewInstance.GetWeapon());

                Weapon newWeapon = viewInstance.GetWeapon();

                newWeapon.Muzzle.InitPool(_ignoreRaycastLayerMask, newWeapon.MuzzleFlash, _crossHairTransform, _hitEffect);

                
               
                newWeapon.IsActive = isActivate;
                
                newWeapon.WeaponObject.SetActive(true);


            }
            else
            {
                //Already - weapon can be refreshed
            }
        }
         
    }
}