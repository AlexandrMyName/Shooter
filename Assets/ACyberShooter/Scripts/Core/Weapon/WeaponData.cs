using ShootingSystem;
using System;
using System.Collections.Generic;
using System.Linq;

#if UNITY_EDITOR
using UnityEditor.Animations;
#endif

using UnityEngine;
using Views;

namespace Core
{

    [RequireComponent(typeof(Animator))]
    public class WeaponData : MonoBehaviour
    {

        [HideInInspector] public List<Weapon> Weapons = new List<Weapon>();
        [field: SerializeField] public List<WeaponModel_View> _weaponViewsFab { get; set; }

        [Header("Base controller - ovveridable! Animations - tackelage!")]
        [Space, SerializeField] private string _emptyWeaponNameClip = "EmptyWeapon"; 
        private Animator _animator;
        private AnimatorOverrideController _ovverideController;

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

            _animator ??= GetComponent<Animator>();
            _ovverideController = _animator.runtimeAnimatorController as AnimatorOverrideController;
            

            _weaponViewsFab.ForEach(weaponView => AddWeapon(weaponView,false));

            if(Weapons.Count > 0)
            {
                CurrentWeapon = Weapons[0];
                ActivateCurrentWeapon();
            }
          
             
        }


        public void AddWeapon(WeaponModel_View weaponView, bool isActivate)
        {

            Weapon addedWeapon = weaponView.GetWeapon();
            Weapon findedWeapon = Weapons.Where(weap => weap.Type != addedWeapon.Type).FirstOrDefault();

            if(findedWeapon == null)
            {
                
                var viewInstance = GameObject.Instantiate(weaponView, _weaponsRoot);
                Weapons.Add(viewInstance.GetWeapon());

                Weapon newWeapon = viewInstance.GetWeapon();

                newWeapon.Muzzle.InitPool(_ignoreRaycastLayerMask, newWeapon.MuzzleFlash, _crossHairTransform, _hitEffect);

                if (isActivate)
                {
                    Weapons.ForEach(weapon => { 
                        weapon.WeaponObject.SetActive(!isActivate);
                        weapon.Muzzle.Disable();
                    });
                     
                }
                
                newWeapon.WeaponObject.SetActive(isActivate);

                _animator.SetLayerWeight(1, 1.0f);
            }
            else
            {
                //Already 
            }
        }


        public void ActivateCurrentWeapon()
        {

            if (CurrentWeapon == null) return;

            CurrentWeapon.WeaponObject.SetActive(true);
            CurrentWeapon.Muzzle.Activate();
            Invoke(nameof(SetAnimationDelay), 0.001f);
        }

        private void SetAnimationDelay()
        {
           
            _ovverideController[_emptyWeaponNameClip] = CurrentWeapon._weaponClip;
        }


#if UNITY_EDITOR
        [ContextMenu("SaveWeaponPos")]
        private void SaveWeaponPose()
        {

            if (CurrentWeapon == null) return;

            GameObjectRecorder recorder = new GameObjectRecorder(_playerRoot.gameObject);

            recorder.BindComponentsOfType<Transform>(CurrentWeapon.WeaponObject, false);
            recorder.BindComponentsOfType<Transform>(CurrentWeapon.LeftHandTarget.gameObject, false);
            recorder.BindComponentsOfType<Transform>(CurrentWeapon.RightHandTarget.gameObject, false);
  
            recorder.TakeSnapshot(0.0f); // one frame
            recorder.SaveToClip(CurrentWeapon._weaponClip);
        }
    }
#endif
     
}