using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bot_AnimatorIK : MonoBehaviour
{
    public string AnimationID = "OnWing";

    public bool CanPlay;


    private void Start()
    {
         
            GetComponent<Animator>().SetBool(AnimationID, CanPlay);
       
    }
}
