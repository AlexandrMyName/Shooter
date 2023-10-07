using UnityEngine;


namespace Abstracts
{

    public interface IAnimatorIK
    {

        void SetLayerWeight(int indexLayer, float weight);
        void SetLookAtWeight(float weight, float body, float head, float eyes, float clamp);
        void SetLookAtPosition(Vector3 lookAt);

        void SetFloat(string keyID, float value);
        void SetFloat(string keyID, float value, float delta);

        void SetBool(string keyID, bool value);
        void SetWeaponState(IWeaponType weaponType);

    }
}