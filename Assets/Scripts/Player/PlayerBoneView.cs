using UnityEngine;

namespace Player
{
    public class PlayerBoneView : MonoBehaviour
    {
        [SerializeField] private PlayerView _playerView;

        public PlayerView PlayerView => _playerView;
    }
}
