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
         
        private Transform _groundTransformR;
        private Transform _groundTransformL;
        private Collider _rootCollider;

        private LayerMask _groundLayer;
        
        private float _jumpForce;
        private float _groundCastRadius;
        private float _maxCastDistance;

        private float _defaultColliderHeight;
        private float _jumpColliderHeight;
        private float _colliderHeight;
        

        private RaycastHit hitInfo;

        private Vector2 _movement;
        

        private bool _isJumpPressed;
        private bool _isJumpReady;
        private bool _isGrounded;

        private int _groundAnimatorHash;
        private int _jumpAnimatorHash;

        private float _jumpLeg;


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
            _groundTransformR = components.BaseObject.GetComponent<IPlayer>().ComponentsStorage.GroundCheckerR;
            _groundTransformL = components.BaseObject.GetComponent<IPlayer>().ComponentsStorage.GroundCheckerL;

            _groundLayer = _jumpConfig.GroundLayer;
            _jumpForce = _jumpConfig.JumpForce;
            _groundCastRadius = _jumpConfig.GroundCastRadius;
            _maxCastDistance = _jumpConfig.MaxCastDistance;
        }


        protected override void Start()
        {
            _groundAnimatorHash = Animator.StringToHash("Grounded");
            _jumpAnimatorHash = Animator.StringToHash("Jump");
        }

        
        protected override void Update()
        {
             
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
               
            if (_isJumpPressed && IsGrounded())
            {
                if (!_animatorIK.IsJump)
                {
                    _animatorIK.IsJump = true;
                    _animatorIK.SetBool("IsJump", true);
                    _animatorIK.Y_Velocity = _jumpConfig.MaxVelocity_UP; 
                }
                else
                {
                    _animatorIK.SetBool("IsJump", false);
                    _animatorIK.IsJump = false;
                }
                
            }
            else if (!IsGrounded())
            {
                
                 _animatorIK.Y_Velocity -= _jumpConfig.MaxVelocity_DOWN * Time.fixedDeltaTime;
            }
            else
            {
               
                _animatorIK.SetBool("IsJump", false);
                _animatorIK.Y_Velocity = _animatorIK.Animator.deltaPosition.y;
                _animatorIK.IsJump = false;
                 
            }
        }
        

        private bool IsGrounded()
        {

            Vector3 bound = new Vector3(_collider.bounds.center.x, _collider.bounds.min.y, _collider.bounds.center.z);

            bool isGrounded = Physics.CheckCapsule(_collider.bounds.center, bound, _groundCastRadius, _groundLayer);

            if(!isGrounded)
                isGrounded = Physics.SphereCast(_groundTransformR.position, _groundCastRadius, Vector3.down, out RaycastHit hitInfo, _maxCastDistance, _groundLayer, QueryTriggerInteraction.Ignore);
            if (!isGrounded)
                isGrounded 
                    = Physics.Raycast(_groundTransformR.position, Vector3.down, _maxCastDistance, _groundLayer, QueryTriggerInteraction.Ignore) ||
                             Physics.Raycast(_groundTransformL.position, Vector3.down, _maxCastDistance, _groundLayer, QueryTriggerInteraction.Ignore);
            return isGrounded;
        }
         
    }
}