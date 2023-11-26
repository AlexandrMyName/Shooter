using Abstracts;
using Cinemachine;
using Enums;
using EventBus;
using RootMotion.Dynamics;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;
using UnityEngine.Animations.Rigging;


namespace Core
{
     
    [RequireComponent( typeof(WeaponData))]
    public class AnimatorIK : MonoBehaviour, IAnimatorIK
    {

        [Header("Camera TPS Aiming")]
        [SerializeField] private CinemachineVirtualCamera _aimingCameraTPS;
        [SerializeField] private CinemachineVirtualCamera _mainCameraTPS;
        [Header("Camera FPS Aiming")]
        [SerializeField] private CinemachineVirtualCamera _mainCameraFPS;
        [SerializeField] private GameObject _defaultHeadObject;

        [SerializeField,Space(10)] private StepSoundTracker StepsSoundTracker;

        [Header("IK Animator"), Space(20)]

        [SerializeField] private Animator _animator;
        [SerializeField] private Animator _rigController;
       
        

        [SerializeField, Space] private Rig _handsRig;
        [SerializeField, Space] private Rig _aimRig;

        [SerializeField,Range(0f,1f), Space] private float _aimingDuration;
        [SerializeField] private GameObject _puppetObject;
        [SerializeField] private PuppetMaster _puppetMaster;

        private IComponentsStorage _playerComponents;
        private Rigidbody _rootRigidbody;
        private PlayerInput _input;
        private WeaponData _weaponData;
        private Vector3 _rootMotionDirection;
        private Quaternion _rootMotionRotation;
        private Vector3 _rootMotionVelocity;
        private bool _IsProccessRig;


        private List<IDisposable> _disposables = new();

       [field:SerializeField] public bool FpsCamera { get; set; }

        public float Y_Velocity {

            get=> _rootMotionVelocity.y;
             
            set {
                _rootMotionVelocity.y = value;
            }
        }
         
        public GameObject PuppetObject => _puppetObject;

        public PuppetMaster PuppetMaster => _puppetMaster;
        
        public Animator Animator => _animator;

        
        public bool IsLoseBalance { get; set; }

        public bool IsJump { get; set; }


        public void Dispose()
        {

            ShootingEvents.OnShoot -= SetShootAnimation;
            ShootingEvents.OnReload -= SetReloadAnimation;
            _weaponData.Weapons.ForEach(disposable => disposable.Muzzle.Dispose());
            _disposables.ForEach(disp => disp.Dispose());
        }


        private void Awake()
        {

            IsLoseBalance = false;
            _weaponData ??= GetComponent<WeaponData>();
            _animator ??= GetComponent<Animator>();
            _weaponData.InitData();
        }

 
        private void OnValidate()
        {

            _weaponData ??= GetComponent<WeaponData>();
            _animator ??= GetComponent<Animator>();
        }
         

        public void InitComponent(IComponentsStorage componentStorage, WeaponData weaponData)
        {

            _rootRigidbody = GetComponent<Rigidbody>();
            _playerComponents = componentStorage;
            _weaponData = weaponData;
            InitDefaultWeapon(_weaponData.Weapons);
            UpdateAllCameras();
            ShootingEvents.OnShoot += SetShootAnimation;
            ShootingEvents.OnReload += SetReloadAnimation;
            _input = _playerComponents.Input.PlayerInput;
            
        }


        public void InitDefaultWeapon(List<Weapon> weapons)
        {

            _IsProccessRig = false;
            
            for (int i = weapons.Count - 1; i >= 0; i--)
            {
                Debug.Log("INITIALIZE");
                Weapon weapon = weapons[i];
                
                weapon.WeaponObject.SetActive(true);
                SetRigWeaponState(weapon.Type, false, 1f);
                _rigController.SetBool(weapon.Type.ToString() + "Equip", true);
                 
                weapon.Muzzle.Disable();
                weapon.IsActive = false;
            }
            if(_weaponData.CurrentWeapon == null)
            {
                this.enabled = false;
                return;
            }
            if (_weaponData.CurrentWeapon.Type == IWeaponType.None) return; 
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


        int _swordAnimCounter;
        float _swordAnimRefresher;

        private void Update()
        {

            _swordAnimRefresher += Time.deltaTime;
            if (_swordAnimRefresher > 1.0f)
            {
                _swordAnimCounter = 0;
                _swordAnimRefresher = 0.0f;
            }
            if (_weaponData.CurrentWeapon.Type == IWeaponType.Sword && _input.Player.Shoot.WasPerformedThisFrame())
            {

                string randomState = UnityEngine.Random.Range(0.0f, 1.0f) > .5f ? "1" : "2";

                _swordAnimCounter++;
                _swordAnimRefresher = 0.0f;

                if (_swordAnimCounter == 1)
                {
                    randomState = "1";
                }
                else
                {
                    randomState = "2";
                    _swordAnimCounter = 0;
                }
                _rigController.Play("SwordAttack_0" + randomState, 2, 1);
            }
        }

        private void LateUpdate()
        {
             
             
                UpdateAimingIK();


             
        }

         
        private void OnAnimatorMove()
        {
             
            _rootMotionVelocity.x = _animator.deltaPosition.x;
            _rootMotionVelocity.z = _animator.deltaPosition.z;
             
            float currentSpeed = SetMovableSpeed();

            Vector3 direction = _rootMotionVelocity * currentSpeed * 1000f;
          
            _rootRigidbody.velocity = direction * Time.fixedDeltaTime;
             
            if(!IsJump)
                StepsSoundTracker.PlaySteps(_rootRigidbody.velocity);
        }


        private float SetMovableSpeed()
        {

            float currentSpeed;
            if (_input == null) return 0.0f;
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

            if (weapon.Type == IWeaponType.None || weapon.Type == IWeaponType.Sword) weapon.Muzzle.Disable();
        }


        private void ActivateMuzzle(Weapon weapon)
        {
       
            weapon.IsActive = true;
            weapon.Muzzle.Activate();
        }
         
        public void ChangeHeadObject(GameObject head)
        => _defaultHeadObject = head;
       

        public void SetActiveAimingIK(Weapon currentWeapon, bool isActive)
        {

            if (currentWeapon == null) return;

            bool isAiming =  _input.Player.Aim.IsPressed() ? true : false;
            if (Input.GetKeyDown(KeyCode.V))
            {
                FpsCamera = !FpsCamera;
                 
                UpdateAllCameras();
            }

            if (!FpsCamera)
            {
                if (isAiming)
                {
                    _aimingCameraTPS.gameObject.SetActive(true);
                }
                else
                {
                    _aimingCameraTPS.gameObject.SetActive(false);
                }
            }
            
        }

        private void UpdateAllCameras()
        {

            _playerComponents.CinemachineCameraConfig.FPS_Camera = FpsCamera;

            if (FpsCamera)
            {
                _aimingCameraTPS.gameObject.SetActive(false);
                _mainCameraTPS.gameObject.SetActive(false);

                _mainCameraFPS.gameObject.SetActive(true);

                Observable.Timer(TimeSpan.FromSeconds(0.2f)).Subscribe(_ =>
                {
                    _defaultHeadObject.SetActive(false);
                }).AddTo(_disposables);
            }
            else
            {
                _mainCameraTPS.gameObject.SetActive(true);

                _mainCameraFPS.gameObject.SetActive(false);
                _defaultHeadObject.SetActive(true);
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