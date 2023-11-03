using System;
using Abstracts;
using RootMotion.Dynamics;
using UniRx;
using UnityEngine;


namespace Core
{

    public class PlayerLocomotionSystem : BaseSystem
    {

        IGameComponents _components;
        IAnimatorIK _animatorIK;

        float _speedWalk = 12/3f;
        float _speedRun = 22f/3f;
        float _turnMultiplier = 3f;

        Rigidbody _rb;

        Vector3 _direction = Vector3.zero;
        Vector3 _rotation = Vector3.zero;
 
        private bool _isFallen;
        private Vector3 _initialPosition;

        private PlayerInput _input;
         
        
        
        protected override void Awake(IGameComponents components)
        {

            _components = components;
            _input = components.BaseObject.GetComponent<ComponentsStorage>().Input.PlayerInput;
            _rb = components.BaseObject.GetComponent<Rigidbody>();
            _rotation = Vector3.zero;
            _animatorIK = components.BaseObject.GetComponent<IPlayer>().ComponentsStorage.AnimatorIK;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            _initialPosition = _rb.gameObject.transform.position;
        }


        protected override void Update()
        {
            //Turn

            Quaternion look = Quaternion.Euler(0, _components.MainCamera.transform.eulerAngles.y, 0);

            float turn = _turnMultiplier * Time.deltaTime;

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

            if (_animatorIK.PuppetObject.transform.localPosition.y == 0 ||
                _animatorIK.PuppetMaster.state != PuppetMaster.State.Alive )
            {
                _direction.x = _input.Player.Move.ReadValue<Vector2>().x;
                _direction.z = _input.Player.Move.ReadValue<Vector2>().y;
                _direction.y = 0;
                _animatorIK.SetFloat("Horizontal", _direction.x, 300 * Time.deltaTime);
                _animatorIK.SetFloat("Vertical", _direction.z, 300 * Time.deltaTime);
                _animatorIK.SetBool("IsRun", _input.Player.Accelerate.IsPressed());
            }
            else
            {
                _direction.x = 0;
                _direction.y = 0;
                _animatorIK.SetFloat("Horizontal", _direction.x, 3 * Time.deltaTime);
                _animatorIK.SetFloat("Vertical", _direction.z, 3 * Time.deltaTime);
            }


            
           
        }


        protected override void FixedUpdate()
        {
             
            _direction = _components.BaseObject.transform.TransformDirection(_direction.x, 0, _direction.z);

            CheckFallenState();

            float currentSpeed = 0f;

            if (_input.Player.Accelerate.IsPressed())
            {
                if (Mathf.Abs(_direction.z) > 0 || Mathf.Abs(_direction.x) > 0)
                    currentSpeed = _speedRun;
            }
            else currentSpeed = _speedWalk;

            _direction = (_direction * currentSpeed) * Time.fixedDeltaTime;
           
            Move();

            if (_rb.gameObject.transform.position.y < -100f)
            {
                TeleportToInitialPosition();
            }
        }
        
        
        private void Move()
        {
            _rb.MovePosition(_rb.position + _direction);
        }


        private void TeleportToInitialPosition()
        {
            Debug.LogWarning("Tp");
            _rb.gameObject.transform.position = _initialPosition;
            _animatorIK.PuppetObject.transform.position = _rb.gameObject.transform.position;
            _animatorIK.PuppetObject.transform.rotation = _rb.gameObject.transform.rotation;
            _animatorIK.PuppetMaster.gameObject.transform.localPosition = Vector3.zero;
        }


        private void CheckFallenState()
        {
            if (_animatorIK.PuppetObject.transform.position != _rb.gameObject.transform.position && !_isFallen)
            {
                Debug.LogWarning("Fall");
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