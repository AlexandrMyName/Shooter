using Abstracts;
using System.Collections;
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
        [SerializeField] private Transform _lookAt;

        private float _weight, _body, _head, _eyes, _clamp;

        private Vector3 _lookAtIKpos;

       
        [SerializeField] private IKWeightConfig _weightConfig;
        [SerializeField] private WeaponData _weaponData;

        [SerializeField,Range(0f,1f)] private float _aimingDuration;

        private Weapon _currentWeapon;

        public void Dispose()
        {

            _weaponData.Weapons.ForEach(disposable => disposable.Muzzle.Dispose());
        }


        private void Awake()
        {
            _weaponData ??= GetComponent<WeaponData>();
            _animator ??= GetComponent<Animator>();
            InitAimingWeight();
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


        private void Update(){

            InitAimingWeight();


            if (Input.GetMouseButton(0) || Input.GetMouseButton(1))
            {
                ActivateAiming(_currentWeapon);
            }
            else 
            {
                DisableAiming(_currentWeapon);
            }
             
        }


        private void FixedUpdate() =>  SetLookAtPosition(_lookAt.position);


        #region IK Aiming

        private void InitAimingWeight()
        {

            SetLookAtWeight
                (_weightConfig._weight,
                _weightConfig._body,
                _weightConfig._head,
                _weightConfig._eyes, 
                _weightConfig._clamp);
        }
      
        

        #endregion



        public void SetWeaponState(IWeaponType weaponType)
        {
             
            Weapon weapon = _weaponData.Weapons.Find(weapon => weapon.Type == weaponType);

            if (weapon == null | weapon == _currentWeapon) return;
             
            DeactivateAllWeapons();
 
            ActivateMuzzle(weapon);
              
            _currentWeapon = weapon;
        }


        


        private void InitDefaultWeapon(IWeaponType weaponType) => SetWeaponState(weaponType);
        

        private void ActivateMuzzle(Weapon weapon)
        {
            weapon.weaponObject.SetActive(true);
            weapon.Muzzle.Activate();

            _weaponData.Weapons.ForEach(weapon =>
            {
                weapon.rightHandIK_NoAiming.weight = 0f;
                weapon.leftHandIK_NoAiming.weight = 0f;
            });
        }


        private void ActivateAiming(Weapon weapon)
        {
             
                weapon.leftHandIK_aiming.weight += Time.deltaTime/ _aimingDuration;
                weapon.rightHandIK_aiming.weight += Time.deltaTime/_aimingDuration;

                weapon.rightHandIK_NoAiming.weight -= Time.deltaTime / _aimingDuration;
                weapon.leftHandIK_NoAiming.weight -= Time.deltaTime / _aimingDuration;
             
        }


        private void DisableAiming(Weapon weapon)
        {
             
                weapon.leftHandIK_aiming.weight -= Time.deltaTime / _aimingDuration;
                weapon.rightHandIK_aiming.weight -= Time.deltaTime / _aimingDuration;

                weapon.rightHandIK_NoAiming.weight += Time.deltaTime / _aimingDuration;
                weapon.leftHandIK_NoAiming.weight += Time.deltaTime / _aimingDuration;
             
 
        }
         

        private void DeactivateAllWeapons()
        {

            foreach(var weapon in _weaponData.Weapons)
            {
                weapon.rightHandIK_aiming.weight = 0;
                weapon.leftHandIK_aiming.weight = 0;
                weapon.weaponObject.SetActive(false);
                weapon.Muzzle.Disable();
            }
        }
         

        private void OnAnimatorIK(int layerIndex)
        {

            _animator.SetLookAtWeight(_weight, _body, _head, _eyes, _clamp);

            if (_lookAtIKpos != null)
                _animator.SetLookAtPosition(_lookAtIKpos);

        }
         

        private void OnDestroy() => Dispose();
        
    }
}