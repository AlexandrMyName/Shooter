using System;
using Configs;
using EventBus;
using UniRx;
using UnityEngine;

public class Armor : MonoBehaviour
{
    [SerializeField] public PickupItemConfig _pickupItemConfig;
    public PickupItemConfig PickupItemConfig => _pickupItemConfig;
    private bool _isUsed = false;
    public delegate void CallBack();
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("PlayerRagdoll") && !_isUsed)
        {
            CallBack armoringCallBack = new CallBack(UseArmor);
            PlayerEvents.AddArmor(_pickupItemConfig.ArmorAmount, armoringCallBack);
        }
    }
    public void UseArmor()
    {
        _isUsed = true;
        gameObject.SetActive(false);
        Observable.Timer(TimeSpan.FromSeconds(_pickupItemConfig.ArmorCooldown)).Subscribe(_ => RespawnArmor());
    }

    private void RespawnArmor()
    {
        gameObject.SetActive(true);
        _isUsed = false;
    }
}
