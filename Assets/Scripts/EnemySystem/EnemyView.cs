using Configs;
using UnityEngine;

public class EnemyView : MonoBehaviour
{
    [SerializeField] private EnemyConfig _enemyConfig;
    [SerializeField] private EnemyMovement _enemyMovement;
    [SerializeField] private EnemyAttacking _enemyAttacking;
    [SerializeField] private PlayerView _playerView;

    private int _currentEnemyHP = 50;

    public EnemyConfig EnemyConfig => _enemyConfig;

    public EnemyMovement EnemyMovement => _enemyMovement;

    public EnemyAttacking EnemyAttacking => _enemyAttacking;

    public PlayerView PlayerView
    {
        get => _playerView;
        set => _playerView = value;
    }

    public int EnemyHP
    {
        get => _currentEnemyHP;
        set
        {
            _currentEnemyHP = value;
            if (value <= 0)
            {
                Destroy(gameObject);
                Debug.Log($"{gameObject.name} killed");
            }
        }
    }
    
    private void Start()
    {
        _currentEnemyHP = _enemyConfig.EnemyHp;
    }
    
    
    public void TakeDamage(int damage)
    {
        EnemyHP -= damage;
        Debug.Log(EnemyHP);
    }
}
