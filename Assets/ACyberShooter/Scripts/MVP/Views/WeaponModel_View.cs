using Cinemachine;
using Core;
using UnityEngine;


namespace Views
{
    [RequireComponent(typeof(CinemachineImpulseSource))]
    public class WeaponModel_View : MonoBehaviour
    {
         
        [SerializeField] private Weapon _weapon;


        public Weapon GetWeapon()
        {
            _weapon.RecoilConfig.ImpulseSource = GetComponent<CinemachineImpulseSource>();
            return _weapon;
        }

    }
}