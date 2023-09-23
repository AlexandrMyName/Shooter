using System;
using EventBus;
using TMPro;
using UnityEngine;

public class HealthPanelView : MonoBehaviour
{
    [SerializeField] private TMP_Text _currentHPText;
    [SerializeField] private TMP_Text _maxHPText;


    private void Awake()
    {
        PlayerEvents.OnPlayerSpawned += SetStartHP;
        PlayerEvents.OnUpdateHealthView += SetCurrentHP;
    }

    private void OnDestroy()
    {
        PlayerEvents.OnPlayerSpawned -= SetStartHP;
        PlayerEvents.OnUpdateHealthView -= SetCurrentHP;
    }

    private void SetStartHP(int hp)
    {
        SetCurrentHP(hp);
        SetMaxHP(hp);
    }

    public void SetCurrentHP(int hp)
    {
        _currentHPText.text = hp.ToString();
    }

    public void SetMaxHP(int hp)
    {
        _maxHPText.text = hp.ToString();
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
