using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations.Rigging;


namespace Core
{

    public class NPC_Components : MonoBehaviour
    {

        [field: SerializeField] public bool FriendlyNPC { get; set; }
        [field: SerializeField] public List<NPC_AnimationPoints> AnimationPoints { get; private set; }
        [field: SerializeField] public Animator Animator { get; private set; }
        [field: SerializeField] public NavMeshAgent Agent { get; private set; }
        [field: SerializeField] public Rigidbody Rigidbody { get; private set; }
        [field: SerializeField] public Collider RootCollider { get; private set; }
        [field: SerializeField] public List<Rigidbody> Ragdolls { get; private set; }
        [field: SerializeField] public List<string> Idle_animations { get; private set; }
        [field: SerializeField] public List<string> Walk_animations { get; private set; }

        [field: SerializeField] public bool IsEnemyDetected { get; set; }
        [field: SerializeField] public List<string> EnemyTegs { get; private set; }
        [field: SerializeField] public Rig AimingRig { get; private set; }


        private void OnValidate()
        {

            Agent ??= GetComponent<NavMeshAgent>();
            Animator ??= GetComponent<Animator>();
            RootCollider ??= GetComponent<Collider>();
            Rigidbody ??= GetComponent<Rigidbody>();

            Ragdolls ??= GetComponentsInChildren<Rigidbody>().Where(rb=>rb.gameObject.layer == 9).ToList();
        }

    }

    [Serializable]
    public class NPC_AnimationPoints
    {

        [field: SerializeField] public Transform PatrollPoint { get; set; }
        [field: SerializeField] public string AnimationID { get; set; }

        [field: SerializeField] public bool IsLoopingAnimation { get; set; }

        [field: SerializeField] public float AnimationTime { get; set; }

        [field: SerializeField] public List<GameObject> AnimationObjects { get; set; }


    
    }
}