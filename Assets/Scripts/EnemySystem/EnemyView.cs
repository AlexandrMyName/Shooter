using UnityEngine;

public class EnemyView : MonoBehaviour
{
    [SerializeField] private int _enemyHP = 50;

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

    public void TakeDamage(int damage)
    {
        EnemyHP -= damage;
        Debug.Log(EnemyHP);
    }
}
