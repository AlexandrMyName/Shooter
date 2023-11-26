using SteamAudio;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Vector3 = UnityEngine.Vector3;


namespace Core
{

    [Serializable]
    public class StepSoundTracker  
    {

        public List<Step> StepClips = new();

        [SerializeField] private AudioSource a_audioFoot;
 
        [SerializeField] private AudioClip _audioClip_L;
        [SerializeField] private AudioClip _audioClip_R;

        [SerializeField] private float _pitchOnSprint;


        public void PlaySteps(Vector3 moveDirection)
        {
            
            if (moveDirection.sqrMagnitude > 1f)
            {
                if (!a_audioFoot.isPlaying)
                {
                    if (a_audioFoot.clip != _audioClip_L) a_audioFoot.clip = _audioClip_L;
                    else a_audioFoot.clip = _audioClip_R;
                    a_audioFoot.Play();
                }
            }
            else
            {
               // a_audioFoot.Stop();
            }

            if (Input.GetKey(KeyCode.LeftShift))
            {
                a_audioFoot.pitch = _pitchOnSprint;
               
            }
            else
            {
                a_audioFoot.pitch = 1;
            }
        }
    }

    [Serializable]
    public class Step
    {

        public AudioClip AudioClip;
        public StepMaterial Material;
    }

    public enum StepMaterial : byte
    {

        Parket = 0,
        Wood = 1,
        Glass = 3,
        Titanium = 4,
    }
}