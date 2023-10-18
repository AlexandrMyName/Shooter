using UnityEngine;


namespace Configs
{

    [CreateAssetMenu(fileName = nameof(JumpConfig), menuName = "Configs/" + nameof(JumpConfig))]
    public sealed class JumpConfig : ScriptableObject
    {

        [Header("Jump parameters for tweaking"), Space(20)]
        public string Description;

        [field: SerializeField] public LayerMask GroundLayer{  get; private set; }
        
        [field: SerializeField] public float JumpForce { get; private set; } = 500.0f;
        [field: SerializeField] public float GroundCastRadius { get; private set; } = 0.15f;
        [field: SerializeField] public float MaxCastDistance { get; private set; } = 0.25f;

        
    }
}