using System;
using Abstracts;
using RootMotion.Dynamics;
using UniRx;
using UnityEngine;
using UnityEngine.Animations.Rigging;


namespace Core
{

    public class PlayerMovableSystem : BaseSystem
    {

        IGameComponents _components;
        IAnimatorIK _animatorIK;
        private PlayerInput _input;
        
        float _speedWalk = 12f;
        float _speedRun = 22f;
        float _currentSpeed = 0f;
    
        Rigidbody _rb;

        Vector3 _direction = Vector3.zero;
        Vector3 _rotation = Vector3.zero;
        Vector3 _movement = Vector3.zero;
        [SerializeField] private TwoBoneIKConstraint _rithHand;
        [SerializeField] private TwoBoneIKConstraint _leftHand;
         
        Quaternion _target_Rotation = Quaternion.identity;
        private bool _isFallen;
        private Vector3 _initialPosition;
         
        private void Move()
        {

            _movement = _direction;

            if(_movement.x >= Mathf.Abs(1) && _movement.z >= Mathf.Abs(1))
            {
                _movement.x /= 2;
                _movement.z /= 2;
            }

            _rb.MovePosition(_rb.position + _movement * _currentSpeed *  Time.fixedDeltaTime);

           // if ((Mathf.Abs(_direction.x) > 0 || Mathf.Abs(_direction.y) > 0) || _input.Player.Shoot.IsPressed() || _input.Player.Aim.IsPressed())
               // _rb.MoveRotation(_target_Rotation);
        }


        protected override void Awake(IGameComponents components)
        {

            _components = components;
            _input = _components.BaseObject.GetComponent<IComponentsStorage>().Input.PlayerInput;
            _rb = components.BaseObject.GetComponent<Rigidbody>();
            _rotation = Vector3.zero;
            _animatorIK = components.BaseObject.GetComponent<IPlayer>().ComponentsStorage.AnimatorIK;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            _initialPosition = _rb.gameObject.transform.position;
        }


        protected override void Update()
        {

            if (_animatorIK.IsLoseBalance || _animatorIK.IsJump) return;

            if (_animatorIK.PuppetObject.transform.localPosition.y == 0 ||
                _animatorIK.PuppetMaster.state != PuppetMaster.State.Alive )
            {
                _direction.x = _input.Player.Move.ReadValue<Vector2>().x;
                _direction.z = _input.Player.Move.ReadValue<Vector2>().y;
                 
                _animatorIK.SetFloat("Horizontal", _direction.x, Time.deltaTime);
                _animatorIK.SetFloat("Vertical", _direction.z, Time.deltaTime);
                _animatorIK.SetBool("IsRun", _input.Player.Accelerate.IsPressed() ? true : false);
            }
            else
            {
                _direction.x = 0;
                _direction.y = 0;
                _animatorIK.SetFloat("Horizontal", _direction.x, Time.deltaTime);
                _animatorIK.SetFloat("Vertical", _direction.z, Time.deltaTime);
            }

            _rotation = _components.MainCamera.transform.parent.transform.forward * 1f;
            _rotation.y = 0;

            
        }


        protected override void FixedUpdate()
        {

            if (_animatorIK.IsLoseBalance || _animatorIK.IsJump) return;

            _direction = _rb.transform.TransformDirection(_direction.x, 0, _direction.z);
            
             CheckFallenState();
              
            if (_input.Player.Accelerate.IsPressed())
            {

                if (Mathf.Abs(_direction.z) > 0 || Mathf.Abs(_direction.x) > 0)
                    _currentSpeed = _speedRun;

            }
            else _currentSpeed = _speedWalk;

            _direction = _direction * Time.fixedDeltaTime;
 
         


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