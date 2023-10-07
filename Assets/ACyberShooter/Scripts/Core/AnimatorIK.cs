using Abstracts;
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

      


        private void Awake()
        {
            _weaponData ??= GetComponent<WeaponData>();
            _animator ??= GetComponent<Animator>();
            InitAimingWeight();
            InitDefaultWeapon(_defaultWeapon);
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

            if (weapon == null) return;
            DeactivateAllWeapons();

            switch (weaponType)
            {

                case IWeaponType.Pistol:

                    //+ Animation
                    ActivateWeapon(weapon);

                    break;

                case IWeaponType.Auto:

                    //+ Animation 
                    ActivateWeapon(weapon);

                    break;

                default:
                    DeactivateAllWeapons();
                    break;
            }
        }


        private void InitDefaultWeapon(IWeaponType weaponType) => SetWeaponState(weaponType);
        


        private static void ActivateWeapon(Weapon weapon)
        {

            weapon.leftHandIK.weight = 1;
            weapon.rightHandIK.weight = 1;
            weapon.weaponObject.SetActive(true);
        }


        private void DeactivateAllWeapons()
        {

            foreach(var weapon in _weaponData.Weapons)
            {
                weapon.rightHandIK.weight = 0;
                weapon.leftHandIK.weight = 0;
                weapon.weaponObject.SetActive(false);
            }
        }


     

        private void OnAnimatorIK(int layerIndex)
        {

            _animator.SetLookAtWeight(_weight, _body, _head, _eyes, _clamp);

            if (_lookAtIKpos != null)
                _animator.SetLookAtPosition(_lookAtIKpos);

        }

    }
}