using Configs;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Configs
{


    [CreateAssetMenu(fileName = nameof(CameraConfig), menuName = "Configs/" + nameof(CinemachineCameraConfig))]

    public class CinemachineCameraConfig : ScriptableObject
    {

        public Cinemachine.AxisState Y_Axis;
        public Cinemachine.AxisState X_Axis;
         

    }
}