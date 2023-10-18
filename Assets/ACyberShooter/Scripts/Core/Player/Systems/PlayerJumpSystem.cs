using Abstracts;
using Configs;
using UnityEngine;


namespace Core
{
    
    public sealed class PlayerJumpSystem : BaseSystem
    {
        
        private IGameComponents _components;
        private JumpConfig _jumpConfig;

        private Rigidbody _rigidbody;
        private Animator _animator;
        private Transform _groundTransform;
        private LayerMask _groundLayer;
        
        private float _jumpForce;
        private float _groundCastRadius;
        private float _maxCastDistance;
        

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
            _rigidbody = components.BaseObject.GetComponent<Rigidbody>();
            _animator = components.BaseObject.GetComponent<IPlayer>().ComponentsStorage.AnimatorIK.Animator;
            _jumpConfig = components.BaseObject.GetComponent<IPlayer>().ComponentsStorage.JumpConfig;
            _groundTransform = components.BaseObject.GetComponent<IPlayer>().ComponentsStorage.GroundChecker;
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
            _isJumpPressed = Input.GetButtonDown("Jump");

            if (_isJumpPressed && _isGrounded)
            {
                // Debug.Log($"JUMP [{_isJumpPressed}]");
                _isJumpReady = true;
            }
            
            if (!_isGrounded) {
                _animator.SetFloat(_jumpAnimatorHash, _rigidbody.velocity.y);
            }
        }


        protected override void FixedUpdate()
        {
            // _isGrounded = Physics.SphereCast(_groundTransform.position, _groundCastRadius, Vector3.down, out RaycastHit hitInfo, _maxCastDistance, _groundLayer, QueryTriggerInteraction.Ignore);
            _isGrounded = Physics.Raycast(_groundTransform.position, Vector3.down, _maxCastDistance, _groundLayer,
                QueryTriggerInteraction.Ignore);
            _animator.SetBool(_groundAnimatorHash, _isGrounded);
            
            Debug.DrawRay(_groundTransform.position, Vector3.down * _maxCastDistance);

            if (_isGrounded)
                // Debug.Log("I'm GROUNDED");
            
            if (_isJumpReady && _isGrounded)
            {
                _isJumpReady = false;
                _rigidbody.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
            }
        }
        
        
        protected override void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(_groundTransform.position + Vector3.down * _maxCastDistance, _groundCastRadius);

            if (_isGrounded)
            {
                Gizmos.color = Color.red;
            }
        }
        
        
    }
}