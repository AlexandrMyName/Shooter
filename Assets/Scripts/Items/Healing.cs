using EventBus;
using UnityEngine;

public class Healing : MonoBehaviour
{
    [SerializeField] private int _healingAmount = 10;
    private bool _isUsed = false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("PlayerRagdoll") && !_isUsed)
        {
            PlayerEvents.HealPlayer(_healingAmount);
            _isUsed = true;
            Destroy(gameObject);
        }
    }
}