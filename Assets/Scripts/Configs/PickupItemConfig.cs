using Enums;
using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(fileName = "PickupItemConfig", menuName = "Configs/PickupItemConfig", order = 0)]
    public class PickupItemConfig : ScriptableObject
    {
        [SerializeField] private int _changingAmount = 10;
        [SerializeField] private int _cooldown = 10;
        [SerializeField] private PickUpType _pickUpType;
        public int ChangingAmount => _changingAmount;
        public int Cooldown => _cooldown;
        public PickUpType PickUpType => _pickUpType;
    }
}