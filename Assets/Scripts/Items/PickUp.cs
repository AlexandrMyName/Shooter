using System;
using Configs;
using Enums;
using EventBus;
using UniRx;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    [SerializeField] public PickupItemConfig _pickupItemConfig;
    
    public PickupItemConfig PickupItemConfig => _pickupItemConfig;
    private bool _isUsed = false;
    public delegate void CallBack();
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("PlayerRagdoll") && !_isUsed)
        {
            CallBack pickupCallBack = new CallBack(UsePickup);
            ApplyPickUp(pickupCallBack);
            
        }
    }

    private void ApplyPickUp(CallBack pickupCallBack)
    {
        switch (_pickupItemConfig.PickUpType)
        {
            case PickUpType.Healing:
                PlayerEvents.HealPlayer(_pickupItemConfig.ChangingAmount, pickupCallBack);
                break;
            case PickUpType.Armor:
                PlayerEvents.AddArmor(_pickupItemConfig.ChangingAmount, pickupCallBack);
                break;
            case PickUpType.RifleAmmo:
                PlayerEvents.AddRifleAmmo(_pickupItemConfig.ChangingAmount, pickupCallBack);
                break;
            default:
                break;
        }
        
    }
    public void UsePickup()
    {
        _isUsed = true;
        gameObject.SetActive(false);
        if (_pickupItemConfig.Cooldown > 0)
        {
            Observable.Timer(TimeSpan.FromSeconds(_pickupItemConfig.Cooldown)).Subscribe(_ =>
            {
                gameObject.SetActive(true);
                _isUsed = false;
            });
        }
    }
}
