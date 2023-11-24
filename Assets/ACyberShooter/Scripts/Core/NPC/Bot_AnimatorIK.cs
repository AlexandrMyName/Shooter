using Configs;
using Core;
using SteamAudio;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.AI;

public class Bot_AnimatorIK : MonoBehaviour
{
    public string AnimationID = "OnWing";
    
    private NavMeshAgent _agent;
    public List<Transform> _botPositionPoints = new();
    public float speed;
    public bool _isWalk;
    public bool _isRun;
    public bool _isIdle;
    public bool _isLoop = false;
    public bool CanPlay;
    private Animator _animator;
    [SerializeField] private float _timer = 0.0f;
    [SerializeField] private DoorSystem _doorSystem;
    [SerializeField] private bool _openTheDoorAfterMovableState;
    [SerializeField] private string _animationID_AfterMovableState;
    [SerializeField, Space(20), Header("Triger Extention")] private BotAnimatorIK_ExtentionTrigger _triggerExtention;
    private int _currentIndexPoint = 0;
    private List<IDisposable> _disposables = new();

    private RaycastWeapon _raycastWeapon;
    [SerializeField] private Transform _muzzle;
    [SerializeField] private LayerMask _ignoreLayerMask;
    [SerializeField] private Transform _rayCastWeaponTarget;
    [SerializeField] private List<ParticleSystem> _muzzleFlash;
    [SerializeField] private BulletConfig _bulletConfig;
    [SerializeField] private WeaponRecoilConfig _weaponRecoilConfig;
    [SerializeField] private float _timeShootInterval = 0.3f;

    [SerializeField] private Collider _rootCollider;
    [SerializeField] private Rigidbody _weaponRB;
    [SerializeField] private float _health;
  

    public float Health { get => _health;
        set
        {
            _health = value;
            if(_health <= 0.0f)
            {
                Death();
            }
        }
    }

   
    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();

        _rootCollider = GetComponent<Collider>();
       
      
            if(_triggerExtention.Collider != null)
            {
                _triggerExtention.InitObservable();
        }

        var rbs = transform.gameObject.GetComponentsInChildren<Rigidbody>();
 
        foreach (var rb in rbs)
        {
            if (rb == _weaponRB) continue;
            rb.useGravity = false;
            rb.mass = 1111f;
        }

    }
  

    public void ActivateWeapon()
    {

        if (_muzzle != null)
        {
             
            _raycastWeapon = new RaycastWeapon(_rayCastWeaponTarget, _muzzle.transform, _ignoreLayerMask);



        }else return;

        Observable.Timer(TimeSpan.FromSeconds(_timeShootInterval)).Repeat().Subscribe(_ =>
        {
            Shoot();
        }).AddTo(_disposables);

    }


    private void Shoot()
    {

        if (_raycastWeapon == null) return;

        foreach (var effect in _muzzleFlash)
        {
            effect.Emit(1);
        }
        
        _raycastWeapon.Fire(_bulletConfig, _weaponRecoilConfig);
         
    }


    public void Dispose()
    {
        _disposables.ForEach(disposable => disposable.Dispose());
    }


    public void Death()
    {

        Dispose();
        _rootCollider.enabled = false;
        var rootRB = _rootCollider.GetComponent<Rigidbody>();
        if(rootRB != null)
        {
            rootRB.isKinematic = true;
        }
        _weaponRB.isKinematic = false;
        _animator.enabled = false;
        
    }


    private void Start()
    {

        _animator = GetComponent<Animator>();

        if (CanPlay)
            _animator.SetBool(AnimationID, CanPlay);

        if (_triggerExtention.Collider == null)
        {
           
            Observable.Timer(TimeSpan.FromSeconds(_timer)).Subscribe(_ =>
            {
                SetMovableState();

            }).AddTo(_disposables);
        }
        else
        {
            _triggerExtention.OnCompleted += SetMovableState;
        }
    }


    private void SetMovableState()
    {

        _agent = GetComponent<NavMeshAgent>();

        if (_agent == null) return;
        if (_isWalk == true || _isRun == true)
        {
            _agent.destination = _botPositionPoints[/*_currentIndexPoint*/ 0].position;
            _animator.SetBool("Walk", _isWalk);
            //  _animator.SetBool("Walk", _isWalk);
            if (!_isWalk)
                _animator.SetBool("Run", _isRun);
        }
    }

    private void Update()
    {

        if (_health <= 0) return;
        
        if (_raycastWeapon != null)
        {
            _raycastWeapon.UpdateBullets(Time.deltaTime);

        }

            if (_agent == null) return;
        if (_agent.isActiveAndEnabled == false) return;
       // _agent.destination = _botPositionPoints[_currentIndexPoint].position;
        float distanceToPoint = _agent.remainingDistance;
        if (!_agent.hasPath) return;
        if (!_isLoop)
        {
            
            if (distanceToPoint <= 0.3f && !_agent.pathPending)
            {
               
                if (_botPositionPoints.Count - 1 >= ++_currentIndexPoint)
                {
                   
                    _agent.SetDestination(_botPositionPoints[_currentIndexPoint].position);
                }
                else
                {
                    
                    _animator.SetBool("Idle", true);
                    _animator.SetBool("Walk", false);
                    _animator.SetBool("Run", false);
                    _agent.ResetPath();
                    transform.rotation = _botPositionPoints[_botPositionPoints.Count - 1].rotation;

                     
                    if(_animationID_AfterMovableState != string.Empty)
                    {
                        _agent.enabled = false;
                        _animator.SetTrigger(_animationID_AfterMovableState);
                        transform.position = _botPositionPoints[_botPositionPoints.Count - 1].position;
                        transform.rotation = _botPositionPoints[_botPositionPoints.Count - 1].rotation;
                         
                    }
                    if(_doorSystem != null)
                    {
                        if (_openTheDoorAfterMovableState)
                        {
                            _doorSystem.Open();
                        }
                        else
                        {
                            Debug.Log("Closed");
                            _doorSystem.Close();
                        }
                    }
                }
            }
        }
    }

    private void OnDestroy()
    {

        if(_triggerExtention != null)
            _triggerExtention.OnCompleted -= SetMovableState;

        _disposables.ForEach(disposable => disposable.Dispose());
        _disposables.Clear();
        _triggerExtention.Dispose();
    }


    [Serializable]
    public class BotAnimatorIK_ExtentionTrigger : IDisposable
    {

        [field: SerializeField] public Collider Collider;

        [field: SerializeField] public string TagID;

        [field: SerializeField] public int IterationsCount;

        private List<IDisposable> _disposables = new();

        private int _currentIteractions;

        public Action OnCompleted;


        public void Dispose()
        {

            _disposables.ForEach(disposable => disposable.Dispose());
            _disposables.Clear();
        }


        public void InitObservable()
        {

            _currentIteractions = IterationsCount;
             Collider.OnTriggerEnterAsObservable().Subscribe(
                collider =>
                {
                   
                    if (collider.tag == TagID)
                    {
                         
                        _currentIteractions--;
                         
                        if (_currentIteractions <= 0)
                        {
                            OnCompleted?.Invoke();

                        }
                    }
                }).AddTo(_disposables);
           

        }
          
    }
}
