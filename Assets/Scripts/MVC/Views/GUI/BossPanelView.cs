using System;
using EventBus;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BossPanelView : MonoBehaviour
{
    [SerializeField] private Slider _bossHPSlider;
    [SerializeField] private TMP_Text _timerText;

    private void OnEnable()
    {
        EnemyEvents.OnBossHPUpdated += ChangeBossHPBar;
        EnemyEvents.OnBossSpawnTimerChanged += ChangeBossTimer;
    }

    private void OnDisable()
    {
        EnemyEvents.OnBossHPUpdated -= ChangeBossHPBar;
        EnemyEvents.OnBossSpawnTimerChanged -= ChangeBossTimer;
    }

    private void ChangeBossHPBar(float hp, float maxHP)
    {
        _bossHPSlider.value = hp / maxHP;
    }

    private void ChangeBossTimer(int timeToSpawn)
    {
        TimeSpan t = TimeSpan.FromSeconds(timeToSpawn);
        string timeString = string.Format("{1:D2}:{2:D2}",
            t.Hours,
            t.Minutes,
            t.Seconds,
            t.Milliseconds);

        _timerText.text = timeString;
    }

    public void HideHPBar()
    {
        _bossHPSlider.gameObject.SetActive(false);
    }
    
    public void ShowHPBar()
    {
        _bossHPSlider.gameObject.SetActive(true);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
