using Abstracts;
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;


namespace Core
{

    public class AI_FriendlySystem : BaseSystem, IDisposable
    {

        private NPC_Components NPC_Components;
        private List<NPC_AnimationPoints> _animationOfPoints;
        private List<IDisposable> _disposables = new();

        private int _currentPointIndex;
        private bool _isMoving;
        private bool _isAwait;

        public void Dispose()
        {
           _disposables.ForEach(disposable => disposable.Dispose());
        }


        protected override void Awake(IGameComponents components)
        {
            
            NPC_Components = components.BaseTransform.GetComponent<NPC_Components>();
            // NPC_Components.Animator.SetBool("Idle", true); return;
            _animationOfPoints = NPC_Components.AnimationPoints;
            NPC_Components.Animator.SetBool(NPC_Components.Walk_animations[0], false);
            NextPointMove();
        }

        private void NextPointMove()
        {

            _currentPointIndex++; 

            if(_currentPointIndex >= _animationOfPoints.Count)
            {
                _currentPointIndex = 0;
            }

            _isAwait = false;
            NPC_Components.Agent.destination = _animationOfPoints[_currentPointIndex].PatrollPoint.position;
            _isMoving = true;
           
            Debug.Log("Refreshed");

        }


        protected override void Update()
        {

            if (_isAwait) return;

            if (_isMoving)
            {
                if(NPC_Components.Agent.remainingDistance <= .3f)
                {
                    _isMoving = false;
                    
                    OnAnimationPoint();
                }
            }
            NPC_Components.Animator.SetBool(NPC_Components.Walk_animations[0], _isMoving);
        }


        private void OnAnimationPoint()
        {

            _isAwait = true;

            var point = _animationOfPoints[_currentPointIndex];

            NPC_Components.Animator.SetBool(point.AnimationID, true);
            point.AnimationObjects.ForEach(obj => obj.SetActive(true));

            if (point.IsLoopingAnimation || !point.IsLoopingAnimation) {

                Observable.Timer(TimeSpan.FromSeconds(point.AnimationTime)).Subscribe(_ =>
                {
                    point.AnimationObjects.ForEach(obj => obj.SetActive(false));
                    NPC_Components.Animator.SetBool(point.AnimationID, false);
                  
                    _isAwait = false;

                    NextPointMove();

                }).AddTo(_disposables);
            }
            else
            {

            }
        }
    }
}