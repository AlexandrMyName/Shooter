using EventBus;
using TMPro;
using UnityEngine;

namespace UI
{
    public class AmmoPanelView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _currentAmmoInMagazineText;
        [SerializeField] private TMP_Text _currentAmmoText;


        private void Awake()
        {
            ShootingEvents.OnAmmoCountChanged += SetAmmo;
            ShootingEvents.OnAmmoCountInMagazineChanged += SetAmmoInMagazine;
        }

        private void OnDestroy()
        {
            ShootingEvents.OnAmmoCountChanged -= SetAmmo;
            ShootingEvents.OnAmmoCountInMagazineChanged -= SetAmmoInMagazine;
        }

        private void SetAmmoInMagazine(int ammoInMagazine)
        {
            _currentAmmoInMagazineText.text = ammoInMagazine.ToString();
        }

        private void SetAmmo(int ammo)
        {
            _currentAmmoText.text = ammo.ToString();
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
