using Abstracts;
using RootMotion.Dynamics;
using ShootingSystem;
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

            //SetRigWeaponState(_weaponData.CurrentWeapon.Type, false);

            StartCoroutine(SwitchWeaponRig(_weaponData.CurrentWeapon));

            _weaponData.Weapons.ForEach(weap =>
            {
                weap.Muzzle.Disable();
            });
            

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


        private void SetRigWeaponState(IWeaponType weaponType, bool isEquiped)
        {
            if (_rigController != null) {

                string equipmentState = isEquiped ? "Equip" : "Unequip"; 
                _rigController.Play(weaponType.ToString() + equipmentState, 0);

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
            if (weapon.Type == _weaponData.CurrentWeapon.Type) return;
            DeactivateAllWeapons();
             
            
            StartCoroutine(SwitchWeaponRig(weapon));
             
           
        }
         
         
        private IEnumerator ActivateWeaponRig(Weapon weapon)
        {
             
            bool isAllready = weapon.Type == _weaponData.CurrentWeapon.Type;

            if (!isAllready)
            {
                SetRigWeaponState(weapon.Type, true);
                _rigController.SetBool(_weaponData.CurrentWeapon.Type.ToString() + "Equip",true);

                do
                {
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


        private IEnumerator SwitchWeaponRig(Weapon weapon)
        {

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