using System.Collections;
using System.Collections.Generic;
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

    private int _currentIndexPoint = 0;


    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
    }
    private void Start()
    {

        _animator = GetComponent<Animator>();
        _agent = GetComponent<NavMeshAgent>();
        if (CanPlay)
            _animator.SetBool(AnimationID, CanPlay);
        

        if(_agent == null) return;
        if(_isWalk == true || _isRun == true)
        { 
            _agent.destination = _botPositionPoints[/*_currentIndexPoint*/ 0].position;
            _animator.SetBool("Walk",_isWalk);
          //  _animator.SetBool("Walk", _isWalk);
          if(!_isWalk)
            _animator.SetBool("Run", _isRun);
        }
    }


    private void Update()
    {
        
        if (_agent == null) return;
       // _agent.destination = _botPositionPoints[_currentIndexPoint].position;
        float distanceToPoint = _agent.remainingDistance;
        
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
                }
            }
        }
    }
}
