using UnityEngine;


namespace Configs
{

    [CreateAssetMenu(fileName = nameof(LocomotionConfig), menuName = "Configs/" + nameof(LocomotionConfig))]
    public class LocomotionConfig : ScriptableObject
    {

         public float WalkSpeed = 50f;
         public float RunSpeed = 100f;
         public float TurnMultiplier = 3f;
    }
}