using Core;
using UnityEngine;


namespace Views
{

    public class WeaponModel_View : MonoBehaviour
    {
         
        [SerializeField] private Weapon _weapon;


        public Weapon GetWeapon()
        {
            
            return _weapon;
        }

    }
}