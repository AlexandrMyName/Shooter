using Abstracts;
using RootMotion.Dynamics;
using UnityEngine;
  

namespace Core
{
     
    [RequireComponent(typeof(Animator), typeof(WeaponData))]
    public class AnimatorIK : MonoBehaviour, IAnimatorIK
    {

        [Header("IK Animator"), Space(20)]

        [SerializeField] private Animator _animator;

        [SerializeField] private IWeaponType _defaultWeapon;
        [SerializeField] private Transform _lookAt;

        private float _weight, _body, _head, _eyes, _clamp;

        private Vector3 _lookAtIKpos;
         
         
        [SerializeField] private WeaponData _weaponData;

        [SerializeField,Range(0f,1f)] private float _aimingDuration;
        [SerializeField] private GameObject _puppetObject;
        [SerializeField] private PuppetMaster _puppetMaster;

        private Weapon _currentWeapon;

        public GameObject PuppetObject => _puppetObject;
        public PuppetMaster PuppetMaster => _puppetMaster;


        public void Dispose()
        => _weaponData.Weapons.ForEach(disposable => disposable.Muzzle.Dispose());
        

        private void Awake()
        {
            
            _weaponData ??= GetComponent<WeaponData>();
            _animator ??= GetComponent<Animator>();
            
            InitDefaultWeapon(_defaultWeapon);
            _weaponData.InitData();
        }


        private void OnValidate()
        {

            _weaponData ??= GetComponent<WeaponData>();
            _animator ??= GetComponent<Animator>();
        }


        public void SetLayerWeight(int indexLayer, float weight) => _animator.SetLayerWeight(indexLayer, weight);
           
        
        public void SetLookAtWeight(float weight, float body, float head, float eyes, float clamp)
        {

            _weight = weight;
            _body = body;
            _head = head;
            _eyes = eyes;
            _clamp = clamp;
        }


        public void SetLookAtPosition(Vector3 lookAt) => _lookAtIKpos = lookAt;
        public void SetTrigger(string keyID) => _animator.SetTrigger(keyID);
        public void SetFloat(string keyID, float value) => _animator.SetFloat(keyID, value);
        public void SetFloat(string keyID, float value, float delta) => _animator.SetFloat(keyID, value, 0, delta);
        public void SetBool(string keyID, bool value) => _animator.SetBool(keyID, value);


        private void Update()
        {
             
            UpdateAimingIK();

        }

        private void UpdateAimingIK()
        {
             
            
            if (_currentWeapon.Muzzle.CurrentAmmoInMagazine == 0)
            {
                SetActiveAimingIK(_currentWeapon, false); 
                return;
            }

            if (Input.GetMouseButtonDown(0) && _currentWeapon.Type == IWeaponType.Pistol)
            {
                
                SetActiveAimingIK(_currentWeapon, true);
               
                return;
            }


            if (Input.GetMouseButton(1) || Input.GetMouseButton(0))
            {
              
                    SetActiveAimingIK(_currentWeapon, true);
                
            }
            else
            {
                SetActiveAimingIK(_currentWeapon, false);
            }

          
        }

       
        public void SetWeaponState(IWeaponType weaponType)
        {
             
            Weapon weapon = _weaponData.Weapons.Find(weapon => weapon.Type == weaponType);

            if (weapon == null || weapon == _currentWeapon) return;
             
            DeactivateAllWeapons();
 
            ActivateMuzzle(weapon);
              
            _currentWeapon = weapon;
        }
         

        private void InitDefaultWeapon(IWeaponType weaponType) => SetWeaponState(weaponType);
        

        private void ActivateMuzzle(Weapon weapon)
        {

            weapon.WeaponObject.SetActive(true);
            weapon.Muzzle.Activate();
            weapon.HandsRig.weight = 1f;
  
        }
         

        public void SetActiveAimingIK(Weapon currentWeapon, bool isActive)
        {
            if (isActive)
            {
                currentWeapon.NoAimingRig.weight -= Time.deltaTime / _currentWeapon.RigDuration;
                currentWeapon.AimingRig.weight += Time.deltaTime / _currentWeapon.RigDuration;

            }
            else
            {
                currentWeapon.NoAimingRig.weight += Time.deltaTime / _currentWeapon.RigDuration;
                currentWeapon.AimingRig.weight -= Time.deltaTime / _currentWeapon.RigDuration;
            }
            
        }

         
        private void DeactivateAllWeapons()
        {

            foreach(var weapon in _weaponData.Weapons)
            {
                weapon.AimingRig.weight = 0f;
                weapon.NoAimingRig.weight = 0f;
                weapon.HandsRig.weight = 0f;
                weapon.WeaponObject.SetActive(false);
                weapon.Muzzle.Disable();
            }
        }
         
 
        private void OnDestroy() => Dispose();
        
    }
}