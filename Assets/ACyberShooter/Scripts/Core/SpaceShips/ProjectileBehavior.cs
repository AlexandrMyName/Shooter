using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ProjectileBehavior : MonoBehaviour
{
    [SerializeField] private ParticleSystem _hitEffectFab;



    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Asteroid")
        {
            var effect = GameObject.Instantiate(_hitEffectFab, collision.transform.position,collision.transform.rotation);
            effect.transform.localScale = collision.gameObject.transform.localScale;
            effect.Emit(1);
            Destroy(collision.gameObject);
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
