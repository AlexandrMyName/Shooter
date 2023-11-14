using Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Asteroid : MonoBehaviour
{

    [SerializeField] private List<string> _tagsForDamages = new();

    [SerializeField] private float _takableDamage;
    [SerializeField] private ParticleSystem _visualEffectFab;
    [SerializeField] private GameObject _soundHitEffectFab;


    private void OnCollisionEnter(Collision collision)
    {
        foreach(var tag in _tagsForDamages)
        {
            
            if (collision.gameObject.tag == tag)
            {
                
                var parentTransform = collision.transform;
                SpaceShipComponent spaceShipComponent;

                while (parentTransform != null)
                {
                   var VarriableSpaceShipComponent = parentTransform.GetComponent<SpaceShipComponent>();

                    if(VarriableSpaceShipComponent == null)
                    {
                        parentTransform = parentTransform.parent.transform;
                    }
                    else
                    {
                        break;
                    }
                }
                
                 spaceShipComponent = parentTransform.GetComponent<SpaceShipComponent>();

                if(spaceShipComponent != null)
                {
                    spaceShipComponent.Health -= _takableDamage;
                    var visualEffect =  GameObject.Instantiate(_visualEffectFab, collision.transform.position, collision.transform.rotation);
                    var soundEffect = GameObject.Instantiate(_soundHitEffectFab, collision.transform.position, collision.transform.rotation);
                    Destroy(visualEffect, 7f);
                     
                    Destroy(soundEffect, 7f);
                    Destroy(gameObject);

                }
            }
        }
    }

}
