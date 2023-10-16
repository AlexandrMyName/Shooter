using RootMotion;
using UnityEngine;
 

public class Sss : MonoBehaviour
{

    private void OnParticleCollision(GameObject other)
    {
        Debug.Log(other.gameObject.name);
    }

    private void OnParticleTrigger()
    {
        Debug.Log( "s");

 
    }
}
