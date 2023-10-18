using Abstracts;
using RootMotion.Dynamics;
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

         

        public GameObject PuppetObject => _puppetObject;
        public PuppetMaster PuppetMaster => _puppetMaster;


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

            SetRigWeaponState();


        }


        private void OnValidate()
        {

            _weaponData ??= GetComponent<WeaponData>();
            _animator ??= GetComponent<Animator>();
        }

         
        public void SetLayerWeight(int indexLayer, float weight) => _animator.SetLayerWeight(indexLayer, weight);
           
        
     
        public void SetTrigger(string keyID) => _animator.SetTrigger(keyID);
        public void SetFloat(string keyID, float value) => _animator.SetFloat(keyID, value);
        public void SetFloat(string keyID, float value, float delta) => _animator.SetFloat(keyID, value, 0, delta);
        public void SetBool(string keyID, bool value) => _animator.SetBool(keyID, value);


        private void Update()
        {
             
             if(_weaponData.CurrentWeapon != null)
                UpdateAimingIK();
              
        }


        private void SetRigWeaponState()
        {
            if (_rigController != null)
                _rigController.Play(_weaponData.CurrentWeapon.Type.ToString() + "Unequip", 0);
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
             
            Weapon weapon = _weaponData.Weapons.Where(weapon => weapon.Type == weaponType).FirstOrDefault();

            Debug.Log($"{weapon == null} Weapon is null | {weapon == _weaponData.CurrentWeapon}");

            if (weapon == null) return;

            var currentWeaponActiveState = _weaponData.CurrentWeapon.IsActive;

            DeactivateAllWeapons();

            _rigController.SetBool(_weaponData.CurrentWeapon.Type.ToString() + "Equip", false);
              
            _rigController.SetBool(weapon.Type.ToString() + "Equip", !weapon.IsActive);
             
            weapon.IsActive = !weapon.IsActive;
             
            if(weapon.IsActive)
            ActivateMuzzle(weapon);
            else
            {
                 
            }

            _weaponData.CurrentWeapon = weapon;
        }
         
         
        private void ActivateMuzzle(Weapon weapon)
        {
       
             //weapon.WeaponObject.SetActive(true);
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
               // weapon.AimingRig.weight = 0f;
               // weapon.NoAimingRig.weight = 0f;
               // weapon.HandsRig.weight = 0f;
               // weapon.WeaponObject.SetActive(false);
                weapon.Muzzle.Disable();
            }
        }
         
 
        private void OnDestroy() => Dispose();
        
    }
}