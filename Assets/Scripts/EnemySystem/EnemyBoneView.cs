using UnityEngine;

public class EnemyBoneView : MonoBehaviour
{
    [SerializeField] private EnemyView _enemyView;

    public EnemyView EnemyView => _enemyView;
}
