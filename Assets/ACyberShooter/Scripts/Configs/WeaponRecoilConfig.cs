using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = nameof(WeaponRecoilConfig), menuName = "Configs/"+ nameof(WeaponRecoilConfig))]
public class WeaponRecoilConfig : ScriptableObject
{
    [SerializeField] private float X_recoil;
    [SerializeField] private float Y_recoil;
    [SerializeField] private float Z_recoil;

    private Vector3 recoil = new();
    
    public Vector3 GetRecoil()
    {

        recoil.x = X_recoil;
        recoil.y = Y_recoil;
        recoil.z = Z_recoil;
        return recoil;
    }
}
