using Configs;
using Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class BossInterectiveSystem : MonoBehaviour
{

    [SerializeField] private Transform _aimTarget;
    [SerializeField] private float _timeShootInterval;
    [SerializeField] private BulletConfig _bulletConfig;
    [SerializeField] private List<Bot_AnimatorIK> _botsNPC = new();
    [SerializeField] private List<Rigidbody> _ragdolls = new();

    [SerializeField] private List<Transform> _targets = new();
    [SerializeField] private int _currentTargetIndex;

    [SerializeField] private LayerMask _ignoreLayerMask;

    [SerializeField] private Transform _muzzleTransform;
    [SerializeField] private float _health = 1000f;
    private RaycastWeapon _rayCastWeapon;
    [SerializeField] private WeaponRecoilConfig _recoilConfig;

    private List<IDisposable> _disposables = new();


    public float Health
    {
        get => _health;
        set
        {
            _health = value;
            if (_health <= 0.0f)
            {
                Death();
            }
        }
    }

    public void Death()
    {

        GetComponent<Animator>().SetTrigger("Death");

    }

    public void DisableAnimator()
    {
        GetComponent<Animator>().enabled = false;
        _ragdolls.ForEach(ragdoll => { 
            ragdoll.isKinematic = false;
        });
        _ragdolls.ForEach(ragdoll => {
                ragdoll.isKinematic = true;
            });
            Dispose();
        _botsNPC.ForEach(bot => { bot.Dispose(); });
        
    }
     

    public void ActivateAiming()
    {

        _rayCastWeapon = new RaycastWeapon(_aimTarget, _muzzleTransform, _ignoreLayerMask, true);

        Observable.Timer(TimeSpan.FromSeconds(_timeShootInterval)).Repeat().Subscribe(_ =>
        {
            Shoot();
        }).AddTo(_disposables);

        Observable.Timer(TimeSpan.FromSeconds(2.3f)).Repeat().Subscribe(_ =>
        {
            _currentTargetIndex = (_currentTargetIndex + 1) % _targets.Count;
        }).AddTo(_disposables);

        _botsNPC.ForEach(bot => bot.ActivateWeapon());
    }


    private void Update()
    {

        if (_disposables.Count <= 0) return;
        _aimTarget.position = Vector3.Lerp(_aimTarget.position, _targets[_currentTargetIndex].position, Time.deltaTime * 4f);
         
        _rayCastWeapon.UpdateBullets(Time.deltaTime);
    }


    private void Shoot()
    {

        _rayCastWeapon.Fire(_bulletConfig, _recoilConfig);
    }


    public void Dispose()
    {
        _disposables.ForEach(disposable=>disposable.Dispose());
        _disposables.Clear();
    }

}
