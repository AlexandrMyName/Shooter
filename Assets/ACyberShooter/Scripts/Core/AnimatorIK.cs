using Abstracts;
using UnityEngine;


namespace Core
{

    [RequireComponent(typeof(Animator))]
    public class AnimatorIK : MonoBehaviour, IAnimatorIK
    {

        private Animator _animator;
        private Vector3 _lookAtIKpos;
        private void Awake() => _animator = GetComponent<Animator>();

        [SerializeField, Range(0, 1f)] float _weight, _body, _head, _eyes, _clamp;


        public void SetLayerWeight(int indexLayer, float weight)
        {

            _animator.SetLayerWeight(indexLayer, weight);
            Debug.Log("Layer set");
        }


        public void SetLookAtWeight(float weight, float body, float head, float eyes, float clamp)
        {

            _weight = weight;
            _body = body;
            _head = head;
            _eyes = eyes;
            _clamp = clamp;
        }

        public void SetLookAtPosition(Vector3 lookAt) => _lookAtIKpos = lookAt;
        public void SetTrigger(string keyID) => _animator.SetTrigger(keyID);
        public void SetFloat(string keyID, float value) => _animator.SetFloat(keyID, value);
        public void SetBool(string keyID, bool value) => _animator.SetBool(keyID, value);


        private void OnAnimatorIK(int layerIndex)
        {
            _animator.SetLookAtWeight(_weight, _body, _head, _eyes, _clamp);
            if (_lookAtIKpos != null)
                _animator.SetLookAtPosition(_lookAtIKpos);
        }
    }
}