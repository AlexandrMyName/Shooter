using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TimeLineActivator : MonoBehaviour
{

    public List<GameObject> _hidenObjects = new();
    public bool CanPlay = false;

    public PlayableDirector _director;

    private void OnTriggerEnter(Collider other)
    {

        if(other.gameObject.tag == "Player")
        {
            Play();
            CanPlay = false;
            _hidenObjects.ForEach(h => h.SetActive(false));
        }
    }

    public void Play()
    {

        if(CanPlay)
            _director.Play();
    }
}
