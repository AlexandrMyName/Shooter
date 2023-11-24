using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{

    public class ObjectTriggerActivator : MonoBehaviour
    {

        [SerializeField] private string _tagActivation;
        [SerializeField] private List<GameObject> _activationObjects = new();
        [SerializeField] private List<GameObject> _disableObjects = new();


        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == _tagActivation)
            {
                _activationObjects.ForEach(obj => obj.SetActive(true));
                _disableObjects.ForEach(obj => obj.SetActive(false));
                gameObject.SetActive(false);
            }
        }
    }
}