using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(fileName = "PickupItemConfig", menuName = "Configs/PickupItemConfig", order = 0)]
    public class PickupItemConfig : ScriptableObject
    {
        [SerializeField] private int _healingAmount = 10;
        [SerializeField] private int _healCooldown = 10;
        [SerializeField] private int _armorAmount = 10;
        [SerializeField] private int _armorCooldown = 10;
        public int HealingAmount => _healingAmount;
        public int HealCooldown => _healCooldown;
        public int ArmorAmount => _armorAmount;
        public int ArmorCooldown => _armorCooldown;
    }
}