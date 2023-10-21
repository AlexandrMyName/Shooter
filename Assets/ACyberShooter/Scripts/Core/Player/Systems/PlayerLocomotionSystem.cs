using System;
using Abstracts;
using RootMotion.Dynamics;
using UniRx;
using UnityEngine;
using UnityEngine.Animations.Rigging;


namespace Core
{

    public class PlayerLocomotionSystem : BaseSystem
    {

        IGameComponents _components;
        IAnimatorIK _animatorIK;

        float _speedWalk = 12f;
        float _speedRun = 22f;

        float _turnMultiplier = 1055f;
        Rigidbody _rb;

        Vector3 _direction = Vector3.zero;
        Vector3 _rotation = Vector3.zero;

        [SerializeField] private TwoBoneIKConstraint _rithHand;
        [SerializeField] private TwoBoneIKConstraint _leftHand;
         
        Quaternion _target_Rotation = Quaternion.identity;
        private bool _isFallen;
        private Vector3 _initialPosition;
         

        private void Move()
        {
             
          //  _rb.MovePosition(_rb.position + _direction * _speedWalk * Time.fixedDeltaTime);
          //
           // if ((Mathf.Abs(_direction.x) > 0 || Mathf.Abs(_direction.z) > 0) || Input.GetMouseButton(1) || Input.GetMouseButton(0))
            //    _rb.MoveRotation(_target_Rotation);
        }


        protected override void Awake(IGameComponents components)
        {

            _components = components;
            _rb = components.BaseObject.GetComponent<Rigidbody>();
            _rotation = Vector3.zero;
            _animatorIK = components.BaseObject.GetComponent<IPlayer>().ComponentsStorage.AnimatorIK;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            _initialPosition = _rb.gameObject.transform.position;
        }


        protected override void Update()
        {
            
            if (_animatorIK.PuppetObject.transform.localPosition.y == 0 ||
                _animatorIK.PuppetMaster.state != PuppetMaster.State.Alive )
            {
                _direction.x = Input.GetAxis("Horizontal");
                _direction.z = Input.GetAxis("Vertical");
            
                _animatorIK.SetFloat("Horizontal", _direction.x, 1/* / Time.deltaTime*/);
                _animatorIK.SetFloat("Vertical", _direction.z, 1 /*/ Time.deltaTime*/);
                _animatorIK.SetBool("IsRun", Input.GetKey(KeyCode.LeftShift) ? true : false);
            }
            else
            {
                _direction.x = 0;
                _direction.y = 0;
                _animatorIK.SetFloat("Horizontal", _direction.x, 1 / Time.deltaTime);
                _animatorIK.SetFloat("Vertical", _direction.z, 1 / Time.deltaTime);
            }

            _direction = _rb.transform.TransformDirection(_direction.x, 0, _direction.z);
            _rotation = Vector3.zero + _components.MainCamera.transform.TransformDirection(Vector3.forward);
            _rotation.y = 0;
 

          
        }


        protected override void FixedUpdate()
        {
             
             CheckFallenState();
            float currentSpeed = 0f;

            if (Input.GetKey(KeyCode.LeftShift))
            {

                if (Mathf.Abs(_direction.z) > 0 || Mathf.Abs(_direction.x) > 0)
                    currentSpeed = _speedRun;

            }
            else currentSpeed = _speedWalk;

            _direction = _direction * currentSpeed * Time.fixedDeltaTime;
 
            Quaternion look = Quaternion.LookRotation(_rotation);



            float turn = _turnMultiplier * Time.deltaTime;

            _target_Rotation
              = Quaternion
                  .RotateTowards(_components.BaseObject.transform.rotation, look, turn);

            Move();

            if (_rb.gameObject.transform.position.y < -100f)
            {
                TeleportToInitialPosition();
            }
        }


        private void TeleportToInitialPosition()
        {
            _rb.gameObject.transform.position = _initialPosition;
            _animatorIK.PuppetObject.transform.position = _rb.gameObject.transform.position;
            _animatorIK.PuppetObject.transform.rotation = _rb.gameObject.transform.rotation;
            _animatorIK.PuppetMaster.gameObject.transform.localPosition = Vector3.zero;
        }


        private void CheckFallenState()
        {
            if (_animatorIK.PuppetObject.transform.position != _rb.gameObject.transform.position && !_isFallen)
            {
                _isFallen = true;
                Observable.Timer(TimeSpan.FromSeconds(2f)).Subscribe(_ =>
                {
                    _animatorIK.PuppetObject.transform.position = _rb.gameObject.transform.position;
                    _animatorIK.PuppetObject.transform.rotation = _rb.gameObject.transform.rotation;
                    _isFallen = false;
                });
            }
        }


    }
}