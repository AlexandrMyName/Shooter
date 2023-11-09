using Cinemachine;
using UnityEngine;


namespace Abstracts
{

    public interface ISpaceShip
    {
        Transform VisualModel { get; set; }
        CinemachineFreeLook Camera { get; set; }

        Rigidbody Rigidbody { get; set; }
        SpaceshipData SpaceshipData { get; set; }
        Transform GetTransform();

        void GetParametrs();



    }
}