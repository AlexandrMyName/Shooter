using System;
using Abstracts;
using Configs;
using RootMotion.Dynamics;
using UniRx;
using UnityEngine;


namespace Core
{

    public class PlayerLocomotionSystem : BaseSystem
    {

        private IGameComponents _components;
        private IComponentsStorage _componentsStorage;
        private IAnimatorIK _animatorIK;
         
        private LocomotionConfig _locomotion;

        private Rigidbody _rb;

        private Vector3 _direction = Vector3.zero;
        private Vector3 _rotation = Vector3.zero;
 
        private bool _isFallen;
        private Vector3 _initialPosition;

        private PlayerInput _input;
          

        protected override void Awake(IGameComponents components)
        {

            _components = components;
            _componentsStorage = components.BaseObject.GetComponent<IComponentsStorage>();
            _input = _componentsStorage.Input.PlayerInput;
            _rb = components.BaseObject.GetComponent<Rigidbody>();
            _rotation = Vector3.zero;
            _animatorIK = _componentsStorage.AnimatorIK;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            _initialPosition = _rb.gameObject.transform.position;
            _locomotion = _componentsStorage.LocomotionConfig;
        }


        protected override void Update()
        {

            if (_animatorIK.IsLoseBalance) return;

            Quaternion look = Quaternion.Euler(0, _components.MainCamera.transform.eulerAngles.y, 0);

            float turn = _locomotion.TurnMultiplier * Time.deltaTime;

            if ((Mathf.Abs(_direction.x) > 0 || Mathf.Abs(_direction.z) > 0) || Input.GetMouseButton(1) || Input.GetMouseButton(0))
            {
                _components.BaseTransform.rotation = Quaternion.Slerp(_components.BaseObject.transform.rotation, look, turn);
                
                if (_direction.x == 0 && _direction.z == 0 && _components.BaseObject.transform.rotation.y != look.y)
                {
                    _animatorIK.Animator.SetLayerWeight(2, 1.0f);
                  
                    string turnAnimationID =  Quaternion.Angle(look , _components.BaseObject.transform.rotation) > 0 ? "RightTurn" : "LeftTurn";
                    

                    if(Mathf.Abs(Quaternion.Angle(look, _components.BaseObject.transform.rotation)) < 10)
                    {
                        _animatorIK.SetBool("RightTurn", false);
                        _animatorIK.SetBool("LeftTurn", false);
                    }
                    else
                    {
                        _animatorIK.SetBool(turnAnimationID, true);
                    }
                        
                }
                else
                {
                    _animatorIK.SetBool("RightTurn", false);
                    _animatorIK.SetBool("LeftTurn", false);
                    _animatorIK.Animator.SetLayerWeight(2, 0.0f);
                }
            }
            else
            {
                _animatorIK.SetBool("RightTurn", false);
                _animatorIK.SetBool("LeftTurn", false);
            }
              
            _direction.x =  _input.Player.Move.ReadValue<Vector2>().x;
            _direction.z =  _input.Player.Move.ReadValue<Vector2>().y;
            _direction.y = 0;

            _animatorIK.SetFloat("Horizontal", _direction.x, Time.deltaTime);
            _animatorIK.SetFloat("Vertical", _direction.z, Time.deltaTime);
            _animatorIK.SetBool("IsRun", _input.Player.Accelerate.IsPressed());
           
            if(!_animatorIK.IsJump && _direction == Vector3.zero)
            {
                _componentsStorage.Foot_IK.enabled = true;
            }
            else
            {
                _componentsStorage.Foot_IK.enabled = false;
            }
        }


        protected override void FixedUpdate()
        {

             if (_animatorIK.IsLoseBalance) return;

            _direction = _components.BaseObject.transform.TransformDirection(_direction.x, 0, _direction.z);
             
            CheckFallenState();
             
            if (_rb.gameObject.transform.position.y < -100f){

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