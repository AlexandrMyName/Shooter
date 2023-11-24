using UnityEngine;


namespace Optimization
{

    public class GpuActivator : MonoBehaviour
    {

        private void Awake()
        {
            
            MaterialPropertyBlock block = new MaterialPropertyBlock();

            GetComponent<MeshRenderer>().SetPropertyBlock(block);
        }
    }
}