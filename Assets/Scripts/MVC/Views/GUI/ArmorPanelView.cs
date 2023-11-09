using EventBus;
using TMPro;
using UnityEngine;

namespace UI
{
    public class ArmorPanelView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _currentArmorText;
        [SerializeField] private TMP_Text _maxArmorText;


        private void Awake()
        {
            PlayerEvents.OnUpdateArmorView += SetCurrentArmor;
        }

        private void OnDestroy()
        {
            PlayerEvents.OnUpdateArmorView -= SetCurrentArmor;
        }

        private void SetCurrentArmor(int armor, int maxArmor)
        {
            SetCurrentArmor(armor);
            SetMaxArmor(maxArmor);
        }

        public void SetCurrentArmor(int armor)
        {
            _currentArmorText.text = armor.ToString();
        }

        public void SetMaxArmor(int armor)
        {
            _maxArmorText.text = armor.ToString();
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}
