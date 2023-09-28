using UnityEngine;


namespace Abstracts
{

    public interface IAnimatorIK
    {

        void SetLayerWeight(int indexLayer, float weight);
        void SetLookAtWeight(float weight, float body, float head, float eyes, float clamp);
        void SetLookAtPosition(Vector3 lookAt);

    }
}