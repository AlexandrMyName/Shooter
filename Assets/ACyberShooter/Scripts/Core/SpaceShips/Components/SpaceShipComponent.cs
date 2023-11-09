using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Abstracts;
using Cinemachine;

namespace Core
{
    public class SpaceShipComponent : MonoBehaviour, ISpaceShip
    {

        [SerializeField] private Transform _rootTransform;
        [field:SerializeField] public CinemachineFreeLook Camera { get; set; }
        [field: SerializeField] public Transform VisualModel { get; set; }
        [field: SerializeField] public SpaceshipData SpaceshipData { get; set; }
        [field: SerializeField] public Rigidbody Rigidbody { get; set; }
        public void GetParametrs()
        {
            throw new System.NotImplementedException();
        }


        public Transform GetTransform()
        {

            return _rootTransform;
        }
    }
}