using EventBus;
using UnityEngine;

namespace Player
{
    public class PlayerView : MonoBehaviour
    {
        [SerializeField] private Transform _playerTransform;
        [SerializeField] private Transform _playerDamagableZoneTransform;
        [SerializeField] private Transform _playerHeadTransform;
        [SerializeField] private int _playerMaxHP = 50;
        [SerializeField] private int _playerMaxArmor = 50;

        private int _currentPlayerHP;
        private int _currentPlayerArmor;
        private int _currentScore;
        private bool _godMode;
        private bool _isDead;

        public Transform PlayerTransform => _playerTransform;
        public Transform PlayerDamagableZoneTransform => _playerDamagableZoneTransform;
        public Transform PlayerHeadTransform => _playerHeadTransform;

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

        public int PlayerArmor
        {
            get => _currentPlayerArmor;
            set
            {
                if (value > 0)
                {
                    _currentPlayerArmor = value;
                }
                else
                {
                    _currentPlayerArmor = 0;
                }
                PlayerEvents.UpdateArmorView(_currentPlayerArmor, _playerMaxArmor);
            }
        }

        private void Start()
        {
            EnemyEvents.OnDead += AddScore;
            PlayerEvents.OnPlayerHealed += TakeHeal;
            PlayerEvents.OnPlayerArmorAdded += TakeArmor;
            PlayerEvents.OnGodMode += GodMode;
            _currentPlayerHP = _playerMaxHP;
            _currentPlayerArmor = 0;
            PlayerEvents.SpawnPlayer(_playerMaxHP);
            PlayerEvents.UpdateArmorView(_currentPlayerArmor, _playerMaxArmor);
        }

        public void TakeDamage(int damage)
        {
            if (!_isDead && !_godMode)
            {
                if (damage <= PlayerArmor)
                {
                    PlayerArmor -= damage;
                }
                else
                {
                    int deltaDamage = damage - PlayerArmor;
                    PlayerArmor -= damage;
                    PlayerHP -= deltaDamage;
                }
            }
        }

        public void TakeHeal(int healAmount)
        {
            if (!_isDead)
            {
                if (PlayerHP + healAmount > _playerMaxHP)
                {
                    PlayerHP = _playerMaxHP;
                }
                else
                {
                    PlayerHP += healAmount;
                }
            }
        }
        public void GodMode(bool godMode)
        {
            _godMode = godMode;
        }

        public void TakeArmor(int armorAmount)
        {
            if (!_isDead)
            {
                if (PlayerArmor + armorAmount > _playerMaxHP)
                {
                    PlayerArmor = _playerMaxArmor;
                }
                else
                {
                    PlayerArmor += armorAmount;
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
            PlayerEvents.OnPlayerArmorAdded -= TakeArmor;
            PlayerEvents.OnGodMode -= GodMode;
        }
    }
}
