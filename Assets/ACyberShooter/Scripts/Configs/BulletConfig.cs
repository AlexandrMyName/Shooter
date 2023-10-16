using UnityEngine;


namespace Configs
{

    [CreateAssetMenu(fileName = nameof(BulletConfig), menuName = "Configs/" + nameof(BulletConfig))]
    public class BulletConfig : ScriptableObject
    {

        [Header("Information of bullet/rocket behavior"), Space(20)]
        public string Description;
      
        [field: SerializeField] public BulletType Type { get; set; }
        [field: SerializeField] public float Speed { get;set; }
        [field: SerializeField] public float Drop { get; set; }
        [field: SerializeField] public float MaxTime { get; set; }

        [field: SerializeField] public float Damage { get; set; }
        [field:SerializeField] public TrailRenderer Traicer { get; set; }

    }

    public enum BulletType : byte
    {
        Default = 0,
    }
}