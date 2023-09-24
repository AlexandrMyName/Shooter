using EventBus;
using UnityEngine;

public class PlayerView : MonoBehaviour
{
    [SerializeField] private Transform _playerTransform;
    [SerializeField] private Transform _playerDamagableZoneTransform;
    [SerializeField] private int _playerHP = 50;

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
            PlayerEvents.UpdateHealthView(_currentPlayerHP);
            if (value <= 0)
            {
                Death();
            }
        }
    }

    private void Start()
    {
        EnemyEvents.OnDead += AddScore;
        PlayerEvents.OnPlayerHealed += TakeHeal;
        _currentPlayerHP = _playerHP;
        PlayerEvents.SpawnPlayer(_playerHP);
    }

    public void TakeDamage(int damage)
    {
        if (!_isDead)
        {
            PlayerHP -= damage;
        }
    }
    public void TakeHeal(int healAmount)
    {
        if (!_isDead)
        {
            if (PlayerHP + healAmount > _playerHP)
            {
                PlayerHP = _playerHP;
            }
            else
            {
                PlayerHP += healAmount;
            }
        }
    }

    private void AddScore()
    {
        _currentScore++;
    }

    private void Death()
    {
        _currentPlayerHP = 0;
        PlayerEvents.UpdateHealthView(_currentPlayerHP);
        _isDead = true;
        Debug.Log($"{gameObject.name} killed");
        PlayerEvents.GameEnded(_currentScore);
        PlayerEvents.PlayerDead();
    }

    private void OnDestroy()
    {
        EnemyEvents.OnDead -= AddScore;
        PlayerEvents.OnPlayerHealed -= TakeHeal;
    }
}
