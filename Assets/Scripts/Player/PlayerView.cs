using EventBus;
using UnityEngine;

public class PlayerView : MonoBehaviour
{
    [SerializeField] private Transform _playerTransform;
    [SerializeField] private Transform _playerDamagableZoneTransform;
    [SerializeField] private int _playerHP = 50;
    [SerializeField] private HealthPanelView _healthPanelView;
    
    private int _currentPlayerHP;
    private int _currentScore;
    private bool _isDead;

    public Transform PlayerTransform => _playerTransform;
    public Transform PlayerDamagableZoneTransform => _playerDamagableZoneTransform;

    public int PlayerHP
    {
        get => _currentPlayerHP;
        set
        {
            _currentPlayerHP = value;
            _healthPanelView.SetCurrentHP(PlayerHP);
            if (value <= 0)
            {
                Death();
            }
        }
    }
    
    private void Start()
    {
        EnemyEvents.OnDead += AddScore;
        _currentPlayerHP = _playerHP;
        if (_healthPanelView != null)
        {
            _healthPanelView.SetMaxHP(_playerHP);
            _healthPanelView.SetCurrentHP(_currentPlayerHP);
        }
    }
    
    
    public void TakeDamage(int damage)
    {
        if (!_isDead)
        {
            PlayerHP -= damage;
            //Debug.Log(PlayerHP);
        }
    }

    private void AddScore()
    {
        _currentScore++;
    }

    private void Death()
    {
        _currentPlayerHP = 0;
        _healthPanelView.SetCurrentHP(PlayerHP);
        //Destroy(gameObject);
        _isDead = true;
        Debug.Log($"{gameObject.name} killed");
        PlayerEvents.GameEnded(_currentScore);
    }

    private void OnDestroy()
    {
        EnemyEvents.OnDead -= AddScore;
    }
}
