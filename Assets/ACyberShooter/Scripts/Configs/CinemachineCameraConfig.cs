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
        public Vector2 Y_AxisRangeTPS = new Vector2(-27.5f, 26.7f);
        public Vector2 Y_AxisRangeFPS = new Vector2(-27.5f, 26.7f);


        public bool FPS_Camera { get; set; }


        public Vector2 GetAxisRange()
        {

            Vector2 axisRange = FPS_Camera ? Y_AxisRangeFPS : Y_AxisRangeTPS;
            return axisRange;
        }
    }
}