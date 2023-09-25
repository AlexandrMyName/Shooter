using System.Collections;
using System.Collections.Generic;
using EventBus;
using UnityEngine;

public class Armor : MonoBehaviour
{
    [SerializeField] private int _armorAmount = 10;
    private bool isUsed = false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("PlayerRagdoll"))
        {
            PlayerEvents.AddArmor(_armorAmount);
            isUsed = true;
            Destroy(gameObject);
        }
    }
}
