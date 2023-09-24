using EventBus;
using UnityEngine;

namespace UI
{
    public class GUIView : MonoBehaviour
    {
        [SerializeField] private PauseView _pauseView;
        [SerializeField] private GameObject _GUIPanel;

        private bool _isPlayerDead;
    
        private void Start()
        {
            PlayerEvents.OnGamePaused += PauseActions;
            PlayerEvents.OnDead += PlayerDeadActions;
        }

        private void OnDestroy()
        {
            PlayerEvents.OnGamePaused -= PauseActions;
            PlayerEvents.OnDead -= PlayerDeadActions;
        }

        private void PlayerDeadActions()
        {
            _pauseView.gameObject.SetActive(false);
            _GUIPanel.SetActive(false);
            _isPlayerDead = true;
        }

        private void PauseActions(bool isPaused)
        {
            if (!_isPlayerDead)
            {
                _GUIPanel.SetActive(!isPaused);
            }
        }
    }
}
