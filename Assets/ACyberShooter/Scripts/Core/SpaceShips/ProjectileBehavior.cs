using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ProjectileBehavior : MonoBehaviour
{

    [SerializeField] private ParticleSystem _hitEffectFab;
    [SerializeField] private GameObject _hitAudioEffectFab;
    
    [SerializeField]


    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Asteroid")
        {
            var effect = GameObject.Instantiate(_hitEffectFab, collision.transform.position,collision.transform.rotation);
            effect.transform.localScale = collision.gameObject.transform.localScale;
            effect.Emit(1);

            var audioSource = GameObject.Instantiate(_hitAudioEffectFab, collision.transform.position, collision.transform.rotation);
            Destroy(audioSource, 10f);
            Destroy(collision.gameObject);
            var parentObject = collision.gameObject.transform;

            while(parentObject.transform.parent != null)
            {
                parentObject = parentObject.parent.transform;
            }
            Debug.Log(parentObject.gameObject.name);
            Destroy(parentObject.gameObject);
            Destroy(gameObject);
        }
        else
        {
            var effect = GameObject.Instantiate(_hitEffectFab, collision.transform.position, collision.transform.rotation);
            effect.transform.localScale = collision.gameObject.transform.localScale;
            effect.Emit(1);
             
            Destroy(gameObject);
        }
    }
}
