using Configs;
using Enums;
using Extentions;
using RootMotion.Demos;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private EnemyView _enemyView;
    [SerializeField] private EnemyMovementBehaviourConfig _movementBehaviour;
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private NavMeshPuppet _navPuppet;
    [SerializeField] private GameObject _goalObject;
    
    private Transform _playerTransform;
    private MovementBehaviour _currentMovementBehaviour;
    private int _currentGoalIndex;
    private Vector3 _currentGoal;
    private bool _isStanding;

    public EnemyView EnemyView => _enemyView;

    public NavMeshPuppet NavPuppet
    {
        get => _navPuppet;
        set => _navPuppet = value;
    }

    public GameObject GoalObject
    {
        get => _goalObject;
        set => _goalObject = value;
    }

    public void ChangeMovementBehaviourToDefault()
    {
        _currentMovementBehaviour = _movementBehaviour.MovementBehaviour;
    }

    public void ChangeMovementBehaviour(MovementBehaviour movementBehaviour)
    {
        _currentMovementBehaviour = movementBehaviour;
    }

    public void ChangeCurrentGoal(Vector3 goal)
    {
        _currentGoal = goal;
    }
    
    private void Start()
    {
        _playerTransform = _enemyView.PlayerView.PlayerTransform;
        ChangeMovementBehaviourToDefault();
        _agent.speed = _movementBehaviour.Speed;
        _agent.acceleration = _movementBehaviour.Acceleration;
        _agent.angularSpeed = _movementBehaviour.RotationSpeed;
        _isStanding = _currentMovementBehaviour == MovementBehaviour.None ||
                      _currentMovementBehaviour == MovementBehaviour.Standing;
        _currentGoalIndex = 0;
        if (_currentMovementBehaviour == MovementBehaviour.ToPlayerPosition)
        {
            //_agent.SetDestination(_playerTransform.position);
            _currentGoal = _playerTransform.position;
            _goalObject.transform.position = _currentGoal;
        }
        else if (_movementBehaviour.MovementBehaviour == MovementBehaviour.ToPoint)
        {
            _currentGoal = _movementBehaviour.GoalPoint;
            _goalObject.transform.position = _currentGoal;
        }
        else if (!_isStanding)
        {
            ChangeDestinationGoal();
        }
        
    }

    private void FixedUpdate()
    {
        _isStanding = _currentMovementBehaviour == MovementBehaviour.None ||
                      _currentMovementBehaviour == MovementBehaviour.Standing;
        if (_isStanding)
        {
            _agent.isStopped = true;
        }
        else
        {
            _agent.isStopped = false;
        }
        
        if (_currentMovementBehaviour == MovementBehaviour.ToPlayerPosition)

        {
            if (_agent.remainingDistance > _movementBehaviour.DistanceFromPlayerToStop)
            {
                _agent.isStopped = false;
                _currentGoal = _playerTransform.position;
                _goalObject.transform.position = _currentGoal;
                //_agent.SetDestination(_playerTransform.position);
            }
            else
            {
                _agent.isStopped = true;
                _currentGoal = _playerTransform.position;
                _goalObject.transform.position = _currentGoal;
                //_agent.SetDestination(_playerTransform.position);
            }
        }
        else if (_agent.remainingDistance < _movementBehaviour.DistanceToChangeGoal && !_isStanding)
        {
            ChangeDestinationGoal();
        }
    }
    
    private void ChangeDestinationGoal()
    { 
        if (_movementBehaviour.MovementBehaviour == MovementBehaviour.Random)
        {
            RandomDestinationChange();
            _currentGoal = _movementBehaviour.GoalsList[_currentGoalIndex];
        }

        _goalObject.transform.position = _currentGoal;
        //_agent.SetDestination(_currentGoal);
    }


    private void RandomDestinationChange()
    {
        int lastIndex = _currentGoalIndex;
        while (_currentGoalIndex == lastIndex)
        {
            _currentGoalIndex = Extention.GetRandomInt(0, _movementBehaviour.GoalsList.Count);
        }
    }
}
