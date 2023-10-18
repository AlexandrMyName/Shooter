using Abstracts;
using Configs;
using UnityEngine;
using UnityEngine.InputSystem;


namespace Core
{
    
    public sealed class PlayerJumpSystem : BaseSystem
    {
        
        private IGameComponents _components;
        private JumpConfig _jumpConfig;
        private PlayerInput _input;

        private Rigidbody _rigidbody;
        private CapsuleCollider _collider;
        
        private Animator _animator;
        private Transform _groundTransformR;
        private Transform _groundTransformL;

        private LayerMask _groundLayer;
        
        private float _jumpForce;
        private float _groundCastRadius;
        private float _maxCastDistance;

        private float _defaultColliderHeight;
        private float _jumpColliderHeight;

        

        private RaycastHit hitInfo;
        

        private bool _isJumpPressed;
        private bool _isJumpReady;
        private bool _isGrounded;

        private int _groundAnimatorHash;
        private int _jumpAnimatorHash;

        private float _jumpLeg;


        protected override void Awake(IGameComponents components)
        {
            _components = components;
            _input = _components.BaseObject.GetComponent<IComponentsStorage>().Input.PlayerInput;
            _rigidbody = components.BaseObject.GetComponent<Rigidbody>();
            _collider = components.BaseObject.GetComponent<CapsuleCollider>();
            _defaultColliderHeight = _collider.height;
            _jumpColliderHeight = _collider.height / 3;
            
            _animator = components.BaseObject.GetComponent<IPlayer>().ComponentsStorage.AnimatorIK.Animator;
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
            _isJumpPressed = _input.Player.Jump.IsPressed();

            if (_isJumpPressed && _isGrounded)
            {
                // Debug.Log($"JUMP [{_isJumpPressed}]");
                _isJumpReady = true;
            }
            
            if (!_isGrounded) {
                _animator.SetFloat(_jumpAnimatorHash, _rigidbody.velocity.y);
                _collider.height = _jumpColliderHeight;
            }
            else
            {
                _collider.height = _defaultColliderHeight;
            }
        }


        protected override void FixedUpdate()
        {
            // _isGrounded = Physics.SphereCast(_groundTransform.position, _groundCastRadius, Vector3.down, out RaycastHit hitInfo, _maxCastDistance, _groundLayer, QueryTriggerInteraction.Ignore);
            _isGrounded = Physics.Raycast(_groundTransformR.position, Vector3.down, _maxCastDistance, _groundLayer, QueryTriggerInteraction.Ignore) || 
                          Physics.Raycast(_groundTransformL.position, Vector3.down, _maxCastDistance, _groundLayer, QueryTriggerInteraction.Ignore);
            
            _animator.SetBool(_groundAnimatorHash, _isGrounded);
            
            Debug.DrawRay(_groundTransformR.position, Vector3.down * _maxCastDistance);
            Debug.DrawRay(_groundTransformL.position, Vector3.down * _maxCastDistance);

            // if (_isGrounded)
            //     Debug.Log("I'm GROUNDED");

            if (_isJumpReady && _isGrounded)
            {
                _isJumpReady = false;
                _rigidbody.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
            }
        }
        
        
        protected override void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(_groundTransformR.position + Vector3.down * _maxCastDistance, _groundCastRadius);

            if (_isGrounded)
            {
                Gizmos.color = Color.red;
            }
        }
        
        
    }
}