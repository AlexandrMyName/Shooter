using Cinemachine;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class DragonBossAnimEvents : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private Animator _newBossInstance;
    [SerializeField] private CinemachineImpulseSource _impuls;
    [SerializeField] private CinemachineVirtualCamera _virtualCamera;

    [SerializeField] private List<GameObject> _newBosses;

    private void Awake()
    {
        var s = _virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        s.m_FrequencyGain = 0.6f;
        s.m_AmplitudeGain = 0.5f;
    }


    public void DeactivateWingLayer()
    {
        _animator.SetLayerWeight(1, 0.08f);
    }

    public void DoShakeCamera()
    {
        _impuls.GenerateImpulse();
    }

    public void DeactivateShake()
    {
        var s = _virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        s.m_FrequencyGain = 0;
        s.m_AmplitudeGain = 0;


        _newBosses.ForEach(smallbos => { 
            
            
            smallbos.GetComponent<Animator>().SetTrigger("Born");
            //smallbos.GetComponent<RigBuilder>().Build();
            
        });
        _newBossInstance.SetTrigger("Create");
    }
}
