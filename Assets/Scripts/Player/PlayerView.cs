using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerView : MonoBehaviour
{
    [SerializeField] private Transform _playerTransform;
    [SerializeField] private int _playerHP = 50;
    
    private int _currentPlayerHP;

    public Transform PlayerTransform => _playerTransform;
    public int PlayerHP
    {
        get => _currentPlayerHP;
        set
        {
            _currentPlayerHP = value;
            if (value <= 0)
            {
                Destroy(gameObject);
                Debug.Log($"{gameObject.name} killed");
            }
        }
    }
    
    private void Start()
    {
        _currentPlayerHP = _playerHP;
    }
    
    
    public void TakeDamage(int damage)
    {
        PlayerHP -= damage;
        Debug.Log(PlayerHP);
    }
}
