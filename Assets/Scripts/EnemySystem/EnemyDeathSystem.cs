using RootMotion.Dynamics;
using UnityEngine;

namespace EnemySystem
{
    public class EnemyDeathSystem : MonoBehaviour
    {
        [SerializeField] private PuppetMaster _puppetMaster;
        [SerializeField] private float _timeToDestroy;
        [SerializeField] private float _timeToDisableCollider;
        [SerializeField] private GameObject _enemyIconObject;
        public void DestroyEnemy()
        {
            _enemyIconObject.SetActive(false);
            Invoke("DisableCollider", _timeToDisableCollider);
            Destroy(transform.parent.gameObject, _timeToDestroy);
        }
        private void DisableCollider()
        {
            Transform[] bodyParts = _puppetMaster.GetComponentsInChildren<Transform>();
            foreach (Transform bodyPart in bodyParts)
            {
                Rigidbody bodyPartRigidbody = bodyPart.GetComponent<Rigidbody>();
                if (bodyPartRigidbody != null)
                {
                    bodyPartRigidbody.isKinematic = true;
                }
                Collider bodyPartCollider = bodyPart.GetComponent<Collider>();
                if (bodyPartCollider != null)
                {
                    bodyPartCollider.enabled = false;
                }
            }
        }
    }
}