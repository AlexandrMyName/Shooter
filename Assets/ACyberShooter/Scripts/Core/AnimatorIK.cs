using Abstracts;
using Enums;
using EventBus;
using RootMotion.Dynamics;
using ShootingSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.InputSystem;

namespace Core
{
     
    [RequireComponent( typeof(WeaponData))]
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

        [Space(10), SerializeField] private ComponentsStorage _playerComponents;
        [SerializeField] private Rigidbody _rootRigidbody;
        private PlayerInput _input;

        private Vector3 _rootMotionDirection;
        private Quaternion _rootMotionRotation;
        private Vector3 _rootMotionVelocity;
        private bool _IsProccessRig;


        public float Y_Velocity {

            get=> _rootMotionVelocity.y;
             
            set {
                _rootMotionVelocity.y = value;
            }
        }
         
        public GameObject PuppetObject => _puppetObject;

        public PuppetMaster PuppetMaster => _puppetMaster;
        
        public Animator Animator => _animator;
         
        public void Dispose() => _weaponData.Weapons.ForEach(disposable => disposable.Muzzle.Dispose());

        public bool IsLoseBalance { get; set; }


        private void Awake()
        {

            IsLoseBalance = false;
            _weaponData ??= GetComponent<WeaponData>();
            _animator ??= GetComponent<Animator>();
            _weaponData.InitData();
        }


        private void Start()
        {
      
            InitDefaultWeapon(_weaponData.Weapons);

            ShootingEvents.OnShoot += SetShootAnimation;
            ShootingEvents.OnReload += SetReloadAnimation;
            _input = _playerComponents.Input.PlayerInput;
           
        }


        private void OnValidate()
        {

            _weaponData ??= GetComponent<WeaponData>();
            _animator ??= GetComponent<Animator>();
        }


        private void InitDefaultWeapon(List<Weapon> weapons)
        {

            _IsProccessRig = false;
            
            for (int i = weapons.Count - 1; i >= 0; i--)
            {
                Weapon weapon = weapons[i];
                
                weapon.WeaponObject.SetActive(true);
                SetRigWeaponState(weapon.Type, false, 1f);
                _rigController.SetBool(weapon.Type.ToString() + "Equip", true);
                 
                weapon.Muzzle.Disable();
                weapon.IsActive = false;
            }

           
            _weaponData.CurrentWeapon = weapons[0];
            _weaponData.CurrentWeapon.IsActive = true;
            _weaponData.CurrentWeapon.Muzzle.Activate();
            SetWeaponState(_weaponData.CurrentWeapon.Type);  
        }
         

        public void SetLayerWeight(int indexLayer, float weight) => _animator.SetLayerWeight(indexLayer, weight);
        public void SetTrigger(string keyID) => _animator.SetTrigger(keyID);
        public void SetFloat(string keyID, float value) => _animator.SetFloat(keyID, value);
        public void SetFloat(string keyID, float value, float delta) => _animator.SetFloat(keyID, value, 0.05f, delta);
        public void SetBool(string keyID, bool value) => _animator.SetBool(keyID, value);


        public void SetRootMotion(Vector3 targetDirection, Quaternion targetRotation)
        {

            _rootMotionDirection = targetDirection;
            _rootMotionRotation = targetRotation;
        }


        public void SetShootAnimation(bool isAct,ShootingType type, float value)
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


        public void OnLooseBalance()
        {
            
            IsLoseBalance = true;
            _rigController.enabled = false;
            _handsRig.weight = 0.0f;
            _aimRig.weight = 0.0f;
            _weaponData.Weapons.ForEach(weapon=>weapon.WeaponObject.SetActive(false));
        }

        public void OnRegainBalance()
        {
            
            IsLoseBalance = false;
            _rigController.enabled = true;
            _handsRig.weight = 1.0f;
            _aimRig.weight = 1.0f;
            _weaponData.Weapons.ForEach(weapon => weapon.WeaponObject.SetActive(true));
        }


        private void LateUpdate()
        {
             
             if(_weaponData.CurrentWeapon != null)
                UpdateAimingIK();


             
        }

         
        private void OnAnimatorMove()
        {

            //_animator.applyRootMotion = true;

            _rootMotionVelocity.x = _animator.deltaPosition.x;
            _rootMotionVelocity.z = _animator.deltaPosition.z;
            
            float currentSpeed = SetMovableSpeed();

            Vector3 direction = _rootMotionVelocity * currentSpeed * 1000f;

            _rootRigidbody.velocity = direction * Time.fixedDeltaTime;
            _rootRigidbody.ResetInertiaTensor();
        }


        private float SetMovableSpeed()
        {

            float currentSpeed;
            if (_input.Player.Accelerate.IsPressed())
            {
                currentSpeed = _playerComponents.LocomotionConfig.RunSpeed;
            }
            else currentSpeed = _playerComponents.LocomotionConfig.WalkSpeed;
            return currentSpeed;
        }


        private void SetRigWeaponState(IWeaponType weaponType, bool isEquiped, float fixedTime = 0)
        {

            if (_rigController != null) {

                string equipmentState = isEquiped ? "Equip" : "Unequip"; 
                _rigController.PlayInFixedTime(weaponType.ToString() + equipmentState,0, fixedTime);

            }
        }
         
        
        private void UpdateAimingIK() => SetActiveAimingIK(_weaponData.CurrentWeapon, true);
        
       
        public void SetWeaponState(IWeaponType weaponType, bool useHolster = true)
        {

             if(_IsProccessRig) return;

            var weapon = _weaponData.Weapons.Where(weapon => weapon.Type == weaponType).FirstOrDefault();
             
            if (weapon == null) return;
             
            if (weapon.Type == _weaponData.CurrentWeapon.Type)
            {
                if (_weaponData.CurrentWeapon.IsActive) return;
            }
   
            DeactivateAllWeapons();
              
            StartCoroutine(SwitchWeaponRig(weapon, useHolster));
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

                do yield return new WaitForEndOfFrame();
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

                do yield return new WaitForEndOfFrame();
                while (_rigController.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f);
                 
                _weaponData.CurrentWeapon.IsActive = false;
            }
           
        }


        private IEnumerator SwitchWeaponRig(Weapon weapon, bool useHolster = true)
        {

            if(useHolster)
                 yield return StartCoroutine(ActivateHolsterRig(weapon));
             
            yield return StartCoroutine(ActivateWeaponRig(weapon));

        }


        private void ActivateMuzzle(Weapon weapon)
        {
       
            weapon.IsActive = true;
            weapon.Muzzle.Activate();
        }
         

        public void SetActiveAimingIK(Weapon currentWeapon, bool isActive)
        {

            bool isAiming =  _input.Player.Aim.IsPressed() ? true : false;
            _animator.SetBool("IsAiming", isAiming);
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