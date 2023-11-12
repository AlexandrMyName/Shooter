using UnityEngine;


namespace Configs
{

    [CreateAssetMenu(fileName = nameof(CameraConfig),menuName = "Configs/"+nameof(CameraConfig))]
    public class CameraConfig : ScriptableObject
    {
        [field: SerializeField] public Vector3 CameraOffSet_Normal { get; set; }
        [field: SerializeField] public Vector3 CameraOffSet_Aiming { get; set; }
        [field: SerializeField] public LayerMask ObstacleLayer { get; set; }
        [field:SerializeField]  public LayerMask NoPlayerLayer { get; set; }
        [field: SerializeField] public LayerMask WithPlayerLayer { get; set; }
        [field: SerializeField] public float CameraSpeedMultiplier { get; set; }

        [field: SerializeField] public float MinDistance { get; set; }

        [field: SerializeField] public float CameraClampMin { get; set; }
        [field: SerializeField] public float CameraClampMax { get; set; }

    }
}