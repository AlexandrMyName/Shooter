using TMPro;
using UnityEngine;

public class AmmoPanelView : MonoBehaviour
{
    [SerializeField] private TMP_Text _currentAmmoInMagazineText;
    [SerializeField] private TMP_Text _currentAmmoText;


    public void SetAmmoInMagazine(int ammoInMagazine)
    {
        _currentAmmoInMagazineText.text = ammoInMagazine.ToString();
    }

    public void SetAmmo(int ammo)
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
