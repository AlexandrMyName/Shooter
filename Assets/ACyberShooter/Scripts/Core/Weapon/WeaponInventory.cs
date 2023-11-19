using UnityEngine;
using Views;


namespace Core
{

    public class WeaponInventory : MonoBehaviour
    {

        [SerializeField] private WeaponData _weaponData;

        [SerializeField] private Transform _weaponRoot;
        [SerializeField] private WeaponModel_View _weaponModelView;
        [SerializeField] private AnimatorIK _animatorIK;


        private void Update()
        {

            if (Input.GetKeyDown(KeyCode.P))
            {
                _weaponData.AddWeapon(_weaponModelView, _weaponRoot);
               
                _animatorIK.InitDefaultWeapon(_weaponData.Weapons);
            }
        }

    }
}