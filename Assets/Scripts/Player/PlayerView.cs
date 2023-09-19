using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerView : MonoBehaviour
{
    [SerializeField] private Transform _playerTransform;
    [SerializeField] private int _playerHP = 50;
    [SerializeField] private HealthPanelView _healthPanelView;
    
    private int _currentPlayerHP;

    public Transform PlayerTransform => _playerTransform;
    public int PlayerHP
    {
        get => _currentPlayerHP;
        set
        {
            _currentPlayerHP = value;
            _healthPanelView.SetCurrentHP(PlayerHP);
            if (value <= 0)
            {
                _currentPlayerHP = 0;
                _healthPanelView.SetCurrentHP(PlayerHP);
                //Destroy(gameObject);
                Debug.Log($"{gameObject.name} killed");
            }
        }
    }
    
    private void Start()
    {
        _currentPlayerHP = _playerHP;
        if (_healthPanelView != null)
        {
            _healthPanelView.SetMaxHP(_playerHP);
            _healthPanelView.SetCurrentHP(_currentPlayerHP);
        }
    }
    
    
    public void TakeDamage(int damage)
    {
        PlayerHP -= damage;
        //Debug.Log(PlayerHP);
    }
}
