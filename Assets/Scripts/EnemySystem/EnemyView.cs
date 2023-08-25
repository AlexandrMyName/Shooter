using UnityEngine;

public class EnemyView : MonoBehaviour
{
    [SerializeField] private int _enemyHP = 50;

    private bool _isMovingLeft;

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

    private void FixedUpdate()
    {
        if (gameObject.transform.position.x >= 0 && !_isMovingLeft)
        {
            _isMovingLeft = true;
        }
        else if (gameObject.transform.position.x <= -4)
        {
            _isMovingLeft = false;
        }

        if (_isMovingLeft)
        {
            gameObject.transform.Translate(-0.05f, 0, 0);
        }
        else
        {
            gameObject.transform.Translate(0.05f, 0, 0);
        }
    }
}
