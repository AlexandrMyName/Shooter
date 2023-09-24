using Enums;
using EventBus;
using UnityEngine;

namespace Animations
{
    public class PlayerAnimationsController : MonoBehaviour
    {
        [SerializeField] private Animator _animator;

        private bool _isShooted;
    
        private void OnEnable()
        {
            ShootingEvents.OnShoot += ChangeShootingState;
        }

        private void OnDisable()
        {
            ShootingEvents.OnShoot -= ChangeShootingState;
        }

        private void ChangeShootingState(bool isShooting, ShootingType shootingType, float animationSpeed)
        {
            _animator.SetFloat("ShootingSpeed", animationSpeed);
            if (shootingType == ShootingType.Single)
            {
                if (isShooting && !_isShooted)
                {
                    _animator.SetBool("IsAiming", isShooting);
                    _animator.SetTrigger("Shoot Single");
                    _isShooted = true;
                }
                else if(!isShooting)
                {
                    _animator.SetBool("IsAiming", isShooting);
                    _isShooted = false;
                }
            }
            else
            {
                _animator.SetBool("IsAiming", isShooting);
                _animator.SetBool("IsShooting", isShooting);
            }
            
        }
    }
}
