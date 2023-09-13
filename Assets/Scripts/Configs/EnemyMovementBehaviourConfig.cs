using System.Collections.Generic;
using Enums;
using NaughtyAttributes;
using UnityEditor;
using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(fileName = "EnemyMovementConfig", menuName = "Configs/EnemySystem/EnemyMovementConfig", order = 1)]
    public class EnemyMovementBehaviourConfig : ScriptableObject
    {
        [SerializeField] private float _speed;
        [SerializeField] private float _acceleration;
        [SerializeField] private float _rotationSpeed;
        [SerializeField] private Vector3 _goalPoint;
        [SerializeField] private List<Vector3> _goalsList;
        [SerializeField] private MovementBehaviour _movementBehaviour;
        [SerializeField] private float _distanceFromPlayerToStop = 10f;
        [SerializeField] private float _distanceToChangeGoal = 0.2f;
        [SerializeField] private bool _isChangingBehaviourNearPlayer;

        public float Speed => _speed;

        public float Acceleration => _acceleration;

        public float RotationSpeed => _rotationSpeed;

        public Vector3 GoalPoint => _goalPoint;

        public List<Vector3> GoalsList => _goalsList;

        public MovementBehaviour MovementBehaviour => _movementBehaviour;

        public float DistanceFromPlayerToStop => _distanceFromPlayerToStop;

        public float DistanceToChangeGoal => _distanceToChangeGoal;

        public bool IsChangingBehaviourNearPlayer => _isChangingBehaviourNearPlayer;
    
        [Button]
        private void AddSelectedAsGoalPoint()
        {
            if (Selection.transforms.Length > 0)
            {
                _goalPoint = Selection.transforms[0].position;
            }
        }
    
        [Button]
        private void AddSelectedAsPatrolPoints()
        {
            foreach (Transform pointTransform in Selection.transforms)
            {
                _goalsList.Add(pointTransform.position);
            }
        }

        [Button]
        private void ClearPatrolPoints()
        {
            _goalsList.Clear();
        }
    
    }
}
