using EventBus;
using UnityEngine;
using Configs;
using UniRx;
using System;

public class Healing : MonoBehaviour
{
    [SerializeField] public PickupItemConfig _pickupItemConfig;
    public PickupItemConfig PickupItemConfig => _pickupItemConfig;
    private bool _isUsed = false;
    public delegate void CallBack();
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("PlayerRagdoll") && !_isUsed)
        {
            CallBack healingCallBack = new CallBack(UseHeal);
            PlayerEvents.HealPlayer(_pickupItemConfig.HealingAmount, healingCallBack);
        }
    }
    public void UseHeal()
    {
        _isUsed = true;
        gameObject.SetActive(false);
        Observable.Timer(TimeSpan.FromSeconds(_pickupItemConfig.HealCooldown)).Subscribe(_ =>
        {
            gameObject.SetActive(true);
            _isUsed = false;
        });
    }
}