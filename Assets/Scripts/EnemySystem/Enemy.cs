using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private int _enemyHP = 5;

    public int EnemyHP
    {
        get => _enemyHP;
        set
        {
            _enemyHP = value;
            if (value <= 0)
            {
                Destroy(gameObject);
                Debug.Log($"{gameObject.name} killed");
            }
        }
    }

    public void TakeDamage()
    {
        EnemyHP--;
    }
}
