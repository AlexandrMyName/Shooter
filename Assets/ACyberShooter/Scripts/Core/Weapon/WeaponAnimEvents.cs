using Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAnimEvents : MonoBehaviour
{

    [SerializeField] private WeaponData _weaponData;
   
    [SerializeField] private Transform LeftHandPoint; 
    [SerializeField] private Transform RightHandPoint;

    private GameObject _magazineInstance;

    public void OnAnimWeapon(string param)
    {

        switch (param)
        {
            case "attachReload":
                OnAnimationAttachMagazine();
                break;

            case "startReload":

                OnAnimationStartReload();
                break;

            case "dropReload":
                OnAnimationDropMagazine();
                 
                break;

            case "takeOnceReload":
                OnAnimationTakeOneMagazine();
                break;

            case "takeSecondReload":
                OnAnimationTakeSecondMagazine();
                break;

            case "endReload":
                OnAnimationEndReload();
                break;

             default:
                break;
        }
    }


    public void OnAnimationDropMagazine()
    {

       if(_magazineInstance != null)
        {
            Destroy(_magazineInstance);
        }
    }

    public void OnAnimationAttachMagazine()
    {

        if(_magazineInstance != null)
        {
            Destroy(_magazineInstance);
            _weaponData.CurrentWeapon.Magazine.SetActive(true);
        }
    }

    public void OnAnimationTakeOneMagazine()
    {
        if (_weaponData.CurrentWeapon.Magazine != null)
        {
            if (_weaponData.CurrentWeapon.Type == Abstracts.IWeaponType.Auto)
            {
                _weaponData.CurrentWeapon.Magazine.SetActive(false);
                _magazineInstance = Instantiate<GameObject>(_weaponData.CurrentWeapon.Magazine, LeftHandPoint);

            }


        }
    }


    public void OnAnimationTakeSecondMagazine()
    {

        if (_weaponData.CurrentWeapon.Magazine != null)
        {
            if (_weaponData.CurrentWeapon.Type == Abstracts.IWeaponType.Auto)
                _magazineInstance = Instantiate<GameObject>(_weaponData.CurrentWeapon.Magazine, LeftHandPoint);


        }
    }


    public void OnAnimationStartReload()
    {
        _weaponData.CurrentWeapon.IsActive = false;
    }

    public void OnAnimationEndReload()
    {
        _weaponData.CurrentWeapon.IsActive = true;
    }
}
