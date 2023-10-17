using Abstracts;
using RootMotion.Dynamics;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace Core
{
     
    [RequireComponent(typeof(Animator), typeof(WeaponData))]
    public class AnimatorIK : MonoBehaviour, IAnimatorIK
    {

        [Header("IK Animator"), Space(20)]

        [SerializeField] private Animator _animator;

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
        => _weaponData.Weapons.ForEach(disposable => disposable.Muzzle.Dispose());
        

        private void Awake()
        {
            
            _weaponData ??= GetComponent<WeaponData>();
            _animator ??= GetComponent<Animator>();
             
            _weaponData.InitData();
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
             
            Weapon weapon = _weaponData.Weapons.Find(weapon => weapon.Type == weaponType);

            if (weapon == null || weapon == _weaponData.CurrentWeapon) return;
             
            DeactivateAllWeapons();
 
            ActivateMuzzle(weapon);

            _weaponData.CurrentWeapon = weapon;
        }
         
         
        private void ActivateMuzzle(Weapon weapon)
        {
            Debug.Log("S");
            weapon.WeaponObject.SetActive(true);
            weapon.Muzzle.Activate();
           // weapon.HandsRig.weight = 1f;
  
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
                weapon.WeaponObject.SetActive(false);
                weapon.Muzzle.Disable();
            }
        }
         
 
        private void OnDestroy() => Dispose();
        
    }
}