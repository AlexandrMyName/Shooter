using UnityEngine;


[CreateAssetMenu(fileName = nameof(IKWeightConfig), menuName = "Configs/"+ nameof(IKWeightConfig))]
public class IKWeightConfig : ScriptableObject
{
    [SerializeField, Range(0, 1f)] public float _weight, _body, _head, _eyes, _clamp;


}
