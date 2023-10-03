using System.Collections;
using System.Collections.Generic;
using EventBus;
using UnityEngine;

public class Armor : MonoBehaviour
{
    [SerializeField] private int _armorAmount = 10;
    private bool _isUsed = false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("PlayerRagdoll") && !_isUsed)
        {
            PlayerEvents.AddArmor(_armorAmount);
            _isUsed = true;
            Destroy(gameObject);
        }
    }
}
