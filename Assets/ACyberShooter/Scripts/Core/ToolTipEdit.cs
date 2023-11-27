using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using Unity.Burst;
using UnityEngine;
using UnityEngine.Jobs;


namespace Core
{

    public class ToolTipEdit : MonoBehaviour
    {

        [SerializeField] private List<ToolTip> _toolTips = new List<ToolTip>();
        [SerializeField] private Transform _cameraTransform;
        private TransformAccessArray _toolTipAccesArray;
        private LookAtCameraJob JobLook;

        private List<IDisposable> _disposables = new();

        [SerializeField] private float _lookAtTimerSeconds = 2f;

        
        private void Start()
        {
           // Debug.Log(transform.forward);return;
            _toolTipAccesArray = new TransformAccessArray(_toolTips.Count, 0);

            _toolTips.ForEach(toolTip =>
            {
                var meshRenderer = toolTip.MeshRenderer;

                meshRenderer.material.mainTexture = toolTip.Texture;
                _toolTipAccesArray.Add(toolTip.PivotTransform);

            });

            Observable.Timer(TimeSpan.FromSeconds(_lookAtTimerSeconds)).Repeat().Subscribe(_ =>
            {
                
                JobLook = new LookAtCameraJob(_cameraTransform.position,_cameraTransform.rotation);

                JobLook.Schedule(_toolTipAccesArray);
                 
            }).AddTo(_disposables);

        }


        private void OnDestroy()
        {

            _disposables.ForEach(disp => disp.Dispose());
            _toolTipAccesArray.Dispose();
        }


        private void OnValidate()
        {

            _cameraTransform ??= Camera.main.transform;
        }

    }
 
    [Serializable]
    public class ToolTip
    {

        public MeshRenderer MeshRenderer;
        public Transform PivotTransform;
        public Texture Texture;

    }


    [BurstCompile]
    public struct LookAtCameraJob : IJobParallelForTransform
    {

        [ReadOnly]
        private Vector3 _cameraPosition;
        private Quaternion _cameraRotation;

        public LookAtCameraJob(Vector3 cameraPos, Quaternion cameraRotation)
        {
            _cameraPosition = cameraPos;
            _cameraRotation = cameraRotation;
        }


        public void Execute(int index, TransformAccess transform)
        {
           // transform.rotation = _cameraRotation;

           
          //  float ang = Vector3.Angle(transform.rotation * Vector3.forward, transform.position - _cameraPosition);

          //  Vector3 rotVect = Vector3.Cross((_cameraPosition - transform.position), transform.rotation * Vector3.forward).normalized;
       
            transform.rotation = Quaternion.LookRotation(_cameraPosition - transform.position, Vector3.up);
        }
    }
}