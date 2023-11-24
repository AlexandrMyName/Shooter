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

        [SerializeField] private GameObject _helmetObject;
        [SerializeField] private GameObject _headObject;


        private void Update()
        {

            //Cheet
            if (Input.GetKeyDown(KeyCode.P))
            {
                GetPistol();
            }
        }


        public void GetPistol()
        {

            _weaponData.AddWeapon(_weaponModelView, _weaponRoot);
          
            _animatorIK.InitDefaultWeapon(_weaponData.Weapons);
            
        }
         
        public void GetHelmet()
        {
            _animatorIK.ChangeHeadObject(_helmetObject);
            _headObject.SetActive(false);

            if(_animatorIK.FpsCamera == false)
                _helmetObject.SetActive(true);
            else
            {
                _helmetObject.SetActive(false);
            }
        }
    }
}