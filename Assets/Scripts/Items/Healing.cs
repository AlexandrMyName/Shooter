using EventBus;
using UnityEngine;

public class Healing : MonoBehaviour
{
    [SerializeField] private int _healingAmount = 10;
    private bool isUsed = false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("PlayerRagdoll"))
        {
            PlayerEvents.HealPlayer(_healingAmount);
            isUsed = true;
            Destroy(gameObject);
        }
    }
}