using Abstracts;
using Player;
using UnityEngine;


namespace Core
{

    [RequireComponent(typeof(Animator))]
    public class AnimatorIK : MonoBehaviour, IAnimatorIK
    {

        [Header("Test IK with Aiming"), Space(20)]

        [SerializeField] private Animator _animator;


        [SerializeField] private Transform _lookAt;

        private float _weight, _body, _head, _eyes, _clamp;

        private Vector3 _lookAtIKpos;

        #region IK Aiming test not for this script

        [SerializeField] private IKWeightConfig _weightConfig;

        [SerializeField] private CameraController _camController;

        [SerializeField] private Transform _leftHandTarget;

        private bool _isAiming;

        [SerializeField] private Vector3 _aimingOffSet;

        [SerializeField] private Vector3 _defaultOffSet;

        #endregion

        private void Awake()
        {

            _animator = GetComponent<Animator>();
            InitAimingWeight();
        }
           

        private void OnValidate() => _animator ??= GetComponent<Animator>();


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
        public void SetBool(string keyID, bool value) => _animator.SetBool(keyID, value);


        private void Update(){


            UpdateAiming();
            UpdateCameraOffSet();

        }


        private void LateUpdate() =>  SetLookAtPosition(_lookAt.position);


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
        private void UpdateAiming()
        {

            if (Input.GetMouseButton(1))
            {

                _isAiming = true;

            }
            else _isAiming = false;

            _animator.SetBool("IsAiming", _isAiming);

        }

        private void UpdateCameraOffSet()
        {

            if (_isAiming){

                _camController.offset = _aimingOffSet;

            }
            else _camController.offset = _defaultOffSet;
        }

        #endregion


        private void OnAnimatorIK(int layerIndex)
        {

            _animator.SetLookAtWeight(_weight, _body, _head, _eyes, _clamp);

            if (_lookAtIKpos != null)
                _animator.SetLookAtPosition(_lookAtIKpos);

            if (_isAiming)
            {
                _animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);
                _animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1);
            }
            else
            {
                _animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 0);
                _animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 0);
            }


            _animator.SetIKPosition(AvatarIKGoal.LeftHand, _leftHandTarget.position);
            _animator.SetIKRotation(AvatarIKGoal.LeftHand, _leftHandTarget.rotation);

        }

    }
}