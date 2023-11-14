using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerActivator : MonoBehaviour
{

    [SerializeField] private List<Transform> _objectsToActivateOnTrigger;


    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            _objectsToActivateOnTrigger.ForEach(obj=>obj.gameObject.SetActive(true));
        }
    }

}
