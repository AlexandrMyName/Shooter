using Abstracts;
using Enums;
using EventBus;
using RootMotion.Dynamics;
using ShootingSystem;
using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace Core
{
     
    [RequireComponent(typeof(Animator), typeof(WeaponData))]
    public class AnimatorIK : MonoBehaviour, IAnimatorIK
    {

        [Header("IK Animator"), Space(20)]

        [SerializeField] private Animator _animator;
        [SerializeField] private Animator _rigController;
        [SerializeField] private IWeaponType _defaultWeapon;
  
        [SerializeField] private WeaponData _weaponData;

        [SerializeField, Space] private Rig _handsRig;
        [SerializeField, Space] private Rig _aimRig;

        [SerializeField,Range(0f,1f), Space] private float _aimingDuration;
        [SerializeField] private GameObject _puppetObject;
        [SerializeField] private PuppetMaster _puppetMaster;


        private bool _IsProccessRig;
        public GameObject PuppetObject => _puppetObject;
        
        public PuppetMaster PuppetMaster => _puppetMaster;
        
        public Animator Animator => _animator;

        
        public void Dispose()
        {

            foreach(var weap in _weaponData.Weapons)
            {
                weap.Muzzle.Dispose();
            }
        }


        private void Awake()
        {
            
            _weaponData ??= GetComponent<WeaponData>();
            _animator ??= GetComponent<Animator>();
             
            _weaponData.InitData();
             
        }
        private void Start()
        {
            //This script need large refactoring
            StartCoroutine(InitDefaultWeapon(_weaponData.Weapons[0],_weaponData.Weapons[1], _weaponData.Weapons[2]));

            ShootingEvents.OnShoot += SetShootAnimation;
            ShootingEvents.OnReload += SetReloadAnimation;
        }

        private void OnValidate()
        {

            _weaponData ??= GetComponent<WeaponData>();
            _animator ??= GetComponent<Animator>();
        }

        private IEnumerator InitDefaultWeapon(Weapon primary, Weapon secondary, Weapon rocketLauncher)
        {

            _IsProccessRig = true;
            primary.WeaponObject.SetActive(false);
            secondary.WeaponObject.SetActive(false);
            rocketLauncher.WeaponObject.SetActive(false);

            SetRigWeaponState(primary.Type, false);
            _rigController.SetBool(primary.Type.ToString() + "Equip", false);
            primary.WeaponObject.SetActive(true);

            do
            {
                yield return null;
            }
            while (_rigController.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f);

            primary.IsActive = false;

            SetRigWeaponState(secondary.Type, false);
            _rigController.SetBool(secondary.Type.ToString() + "Equip", false);
            secondary.WeaponObject.SetActive(true);
            do
            {
                yield return null;
            }
            while (_rigController.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f);

            secondary.IsActive = false;
            rocketLauncher.IsActive = false;
            _IsProccessRig = false;

            rocketLauncher.WeaponObject.SetActive(true);
            yield return StartCoroutine(SwitchWeaponRig(rocketLauncher,true));
            yield return StartCoroutine(SwitchWeaponRig(secondary, true));

            
            secondary.Muzzle.Disable();
            primary.Muzzle.Disable();
            rocketLauncher.Muzzle.Disable();
 
        }
         
        public void SetLayerWeight(int indexLayer, float weight) => _animator.SetLayerWeight(indexLayer, weight);
            
        public void SetTrigger(string keyID) => _animator.SetTrigger(keyID);
        public void SetFloat(string keyID, float value) => _animator.SetFloat(keyID, value);
        public void SetFloat(string keyID, float value, float delta) => _animator.SetFloat(keyID, value, 0, delta);
        public void SetBool(string keyID, bool value) => _animator.SetBool(keyID, value);


        public void SetShootAnimation(bool isAct,ShootingType type,float value)
        {

            if(isAct)
             _rigController.Play(_weaponData.CurrentWeapon.Type.ToString() + "Recoil", 1);
        }


        public void SetReloadAnimation()
        {
             
            if (_weaponData.CurrentWeapon.Type == IWeaponType.Auto)
            {
                _rigController.SetTrigger(_weaponData.CurrentWeapon.Type.ToString() + "Reload");
            }

            if (_weaponData.CurrentWeapon.Type == IWeaponType.Pistol)
            {
                _rigController.SetTrigger(_weaponData.CurrentWeapon.Type.ToString() + "Reload");
            }


        }


        private void Update()
        {
             
             if(_weaponData.CurrentWeapon != null)
                UpdateAimingIK();
        }


        private void SetRigWeaponState(IWeaponType weaponType, bool isEquiped)
        {

            if (_rigController != null) {

                string equipmentState = isEquiped ? "Equip" : "Unequip"; 
                _rigController.PlayInFixedTime(weaponType.ToString() + equipmentState, 0);

            }
        }


        private void UpdateAimingIK()
        {

            SetActiveAimingIK(_weaponData.CurrentWeapon, true);
            return;

            if (_weaponData.CurrentWeapon.Muzzle.CurrentAmmoInMagazine == 0)
            {
                SetActiveAimingIK(_weaponData.CurrentWeapon, false); 
                return;
            }

            if (Input.GetMouseButtonDown(0) && _weaponData.CurrentWeapon.Type == IWeaponType.Pistol)
            {
                
                SetActiveAimingIK(_weaponData.CurrentWeapon, true);
               
                return;
            }


            if (Input.GetMouseButton(1) || Input.GetMouseButton(0))
            {
              
                    SetActiveAimingIK(_weaponData.CurrentWeapon, true);
                
            }
            else
            {
                SetActiveAimingIK(_weaponData.CurrentWeapon, false);
            }

          
        }

       
        public void SetWeaponState(IWeaponType weaponType)
        {

             if(_IsProccessRig) return;
            Weapon weapon = _weaponData.Weapons.Where(weapon => weapon.Type == weaponType).FirstOrDefault();
             
            if (weapon == null) return;
             
            if (weapon.Type == _weaponData.CurrentWeapon.Type)
            {
                if (_weaponData.CurrentWeapon.IsActive) return;
            }
   
            DeactivateAllWeapons();
              
            StartCoroutine(SwitchWeaponRig(weapon));
             
        }
         
         
        private IEnumerator ActivateWeaponRig(Weapon weapon)
        {
            
            bool isAllready = weapon.Type == _weaponData.CurrentWeapon.Type;
            
            if (!isAllready)
            {
                SetRigWeaponState(weapon.Type,  !weapon.IsActive);
                _rigController.SetBool(_weaponData.CurrentWeapon.Type.ToString() + "Equip",true);
                
                do
                {
                    yield return new WaitForSeconds(.1f);
                    yield return new WaitForEndOfFrame();
                }
                while (_rigController.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f);
                
            }
            else if(!_weaponData.CurrentWeapon.IsActive)
            {
                 
                _rigController.SetBool(_weaponData.CurrentWeapon.Type.ToString() + "Equip", true);

                yield return new WaitForSeconds(.1f);

                do
                {
                    yield return new WaitForEndOfFrame();
                }
                while (_rigController.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f);

                 
                _weaponData.CurrentWeapon.IsActive = true;
            }
            
            _weaponData.CurrentWeapon = weapon;
             
            ActivateMuzzle(weapon);
            _IsProccessRig = false;
            
            yield break;
        }


        private IEnumerator ActivateHolsterRig(Weapon weapon)
        {
             
            _IsProccessRig = true;
            
            if (_weaponData.CurrentWeapon.IsActive)
            {
               
                _rigController.SetBool(_weaponData.CurrentWeapon.Type.ToString() + "Equip", false);
           
                yield return new WaitForSeconds(.1f); 

                do
                {
                    yield return new WaitForEndOfFrame();
                }
                while (_rigController.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f);
                 
                _weaponData.CurrentWeapon.IsActive = false;
            }
           
        }


        private IEnumerator SwitchWeaponRig(Weapon weapon, bool ignoreActiveMuzzle = false)
        {

            yield return StartCoroutine(ActivateHolsterRig(weapon));
             
            yield return StartCoroutine(ActivateWeaponRig(weapon));

            if (ignoreActiveMuzzle)
            {
                weapon.Muzzle.Disable();
                weapon.IsActive = false;
              
            }
          
        }


        private void ActivateMuzzle(Weapon weapon)
        {
       
            weapon.IsActive = true;
            weapon.Muzzle.Activate();
        }
         

        public void SetActiveAimingIK(Weapon currentWeapon, bool isActive)
        {
            if (isActive)
            {
               // currentWeapon.NoAimingRig.weight -= Time.deltaTime / _currentWeapon.RigDuration;
              //  currentWeapon.AimingRig.weight += Time.deltaTime / _currentWeapon.RigDuration;

            }
            else
            {
               // currentWeapon.NoAimingRig.weight += Time.deltaTime / _currentWeapon.RigDuration;
               // currentWeapon.AimingRig.weight -= Time.deltaTime / _currentWeapon.RigDuration;
            }
            
        }

         
        private void DeactivateAllWeapons()
        {

            foreach(var weapon in _weaponData.Weapons)
            {
 
                weapon.Muzzle.Disable();
            }
        }
         
 
        private void OnDestroy() => Dispose();
        
    }
}