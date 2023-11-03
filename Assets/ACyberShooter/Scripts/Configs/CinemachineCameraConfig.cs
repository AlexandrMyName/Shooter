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
        [Space(20)]
        public float Sensetivity_Mouse = 50f;
        public float Sensetivity_GamePad = 100f;
        public Vector2 Y_AxisRange = new Vector2(-80, 80);
    }
}