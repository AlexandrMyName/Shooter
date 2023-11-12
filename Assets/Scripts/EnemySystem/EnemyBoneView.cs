using UnityEngine;

namespace EnemySystem
{
    public class EnemyBoneView : MonoBehaviour
    {
        [SerializeField] private EnemyView _enemyView;

        public EnemyView EnemyView => _enemyView;
    }
}
