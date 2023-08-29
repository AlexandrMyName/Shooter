using System.Collections;
using System.Collections.Generic;
using Configs;
using Enums;
using Extentions;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private EnemyView _enemyView;
    [SerializeField] private EnemyMovementBehaviourConfig _movementBehaviour;
    [SerializeField] private NavMeshAgent _agent;
    
    private Transform _playerTransform;
    private MovementBehaviour _currentMovementBehaviour;
    private int _currentGoalIndex;
    private Vector3 _currentGoal;
    private bool _isStanding;

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
            _agent.SetDestination(_playerTransform.position);
        }
        else if (_movementBehaviour.MovementBehaviour == MovementBehaviour.ToPoint)
        {
            _currentGoal = _movementBehaviour.GoalPoint;
        }
        else if (!_isStanding)
        {
            ChangeDestinationGoal();
        }
    }

    private void FixedUpdate()
    {
        if (_currentMovementBehaviour == MovementBehaviour.ToPlayerPosition)

        {
            gameObject.transform.LookAt(_playerTransform);
            if (_agent.remainingDistance > _movementBehaviour.DistanceFromPlayerToStop)
            {
                _agent.isStopped = false;
                _agent.SetDestination(_playerTransform.position);
            }
            else
            {
                _agent.isStopped = true;
                _agent.SetDestination(_playerTransform.position);
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
        _agent.SetDestination(_currentGoal);
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