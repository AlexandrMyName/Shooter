using Abstracts;
using Configs;
using Unity.VisualScripting;
using UnityEngine;


namespace Core
{
    
    public sealed class PlayerJumpSystem : BaseSystem
    {
        
        private IGameComponents _components;
        private IComponentsStorage _componentsStorage;  
        private IAnimatorIK _animatorIK;
        private JumpConfig _jumpConfig;
        private PlayerInput _input;

        private Rigidbody _rigidbody;
        private CapsuleCollider _collider;
  
        private float _defaultColliderHeight;
        private float _jumpColliderHeight;
        private float _colliderHeight;
         
        private Vector2 _movement;
        private LayerMask _groundLayer;

        private bool _isJumpPressed;
      
         
        protected override void Awake(IGameComponents components)
        {

            _components = components;
            _componentsStorage = _components.BaseObject.GetComponent<IComponentsStorage>();
            _input = _componentsStorage.Input.PlayerInput;
            _rigidbody = components.BaseObject.GetComponent<Rigidbody>();
            _collider = components.BaseObject.GetComponent<CapsuleCollider>();
            _defaultColliderHeight = _collider.height;
            _jumpColliderHeight = _collider.height / 2;
            
            _animatorIK = components.BaseObject.GetComponent<IPlayer>().ComponentsStorage.AnimatorIK;
            _jumpConfig = components.BaseObject.GetComponent<IPlayer>().ComponentsStorage.JumpConfig;

            _groundLayer = _jumpConfig.GroundLayer;
           
        }

 
        protected override void Update()
        {
            
            if(_animatorIK.IsLocked) return;

            if (Input.GetKeyDown(KeyCode.LeftControl))
            {
                _animatorIK.Animator.SetTrigger("Dash");
                _rigidbody.AddForce(_rigidbody.transform.forward * 1112f, ForceMode.Acceleration);
            }
            _movement = _input.Player.Move.ReadValue<Vector2>();
            _isJumpPressed = _input.Player.Jump.IsPressed();
             
            if (_animatorIK.IsJump) {

                _colliderHeight = _jumpColliderHeight ;
            }
            else
            {
                _colliderHeight = _defaultColliderHeight;
            }

            _collider.height = Mathf.Lerp(_collider.height, _colliderHeight, Time.deltaTime * 12f);
        }


        protected override void FixedUpdate()
        {

            if (_animatorIK.IsLocked) return;

            if (_isJumpPressed && IsGrounded())
            {
                if (!_animatorIK.IsJump)
                {
                    _animatorIK.IsJump = true;
                    _animatorIK.SetBool("IsJump", true);
                   
                    _animatorIK.Y_Velocity = _jumpConfig.MaxVelocity_UP; 
                }
                
                
            }
            else if (!IsGrounded())
            {
               
                 _animatorIK.Y_Velocity -= _jumpConfig.MaxVelocity_DOWN * Time.fixedDeltaTime;

               
            }
            else if(IsGrounded())
            {
                if (_animatorIK.Y_Velocity < 0) _animatorIK.Y_Velocity = 0;
                _animatorIK.SetBool("IsJump", false);
                _animatorIK.IsJump = false;
                
            }
 
        }
        

        private bool IsGrounded()
        {

            Vector3 bound = new Vector3(_collider.bounds.center.x, _collider.bounds.min.y, _collider.bounds.center.z);

            bool isGrounded = Physics.CheckCapsule(_collider.bounds.center, bound, _jumpConfig.GroundCastRadius, _groundLayer,QueryTriggerInteraction.Ignore);

           
            //if (!isGrounded)
            //    isGrounded
            //        = Physics.Raycast(_groundTransformR.position, Vector3.down, _maxCastDistance, _groundLayer, QueryTriggerInteraction.Ignore) ||
            //                 Physics.Raycast(_groundTransformL.position, Vector3.down, _maxCastDistance, _groundLayer, QueryTriggerInteraction.Ignore);
            return isGrounded;
        }
         
    }
}