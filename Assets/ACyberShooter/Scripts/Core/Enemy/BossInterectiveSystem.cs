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

    [SerializeField] private List<Transform> _targets = new();
    [SerializeField] private int _currentTargetIndex;

    [SerializeField] private LayerMask _ignoreLayerMask;

    [SerializeField] private Transform _muzzleTransform;

    private RaycastWeapon _rayCastWeapon;
    [SerializeField] private WeaponRecoilConfig _recoilConfig;

    private List<IDisposable> _disposables = new();


    private void Start()
    {
        ActivateAiming();
    }

    public void ActivateAiming()
    {

        _rayCastWeapon = new RaycastWeapon(_aimTarget, _muzzleTransform, _ignoreLayerMask);

        Observable.Timer(TimeSpan.FromSeconds(_timeShootInterval)).Repeat().Subscribe(_ =>
        {
            Shoot();
        }).AddTo(_disposables);

        Observable.Timer(TimeSpan.FromSeconds(2.3f)).Repeat().Subscribe(_ =>
        {
            _currentTargetIndex = (_currentTargetIndex + 1) % _targets.Count;
        }).AddTo(_disposables);
    }

    private void Update()
    {

        _aimTarget.position = Vector3.Lerp(_aimTarget.position, _targets[_currentTargetIndex].position, Time.deltaTime * 2f);
         
        _rayCastWeapon.UpdateBullets(Time.deltaTime);
    }


    private void Shoot()
    {

        _rayCastWeapon.Fire(_bulletConfig, _recoilConfig);
    }


}
