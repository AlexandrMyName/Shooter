using EventBus;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
    public class PauseView : MonoBehaviour
    {
        [SerializeField] private Button _continueButton;
        [SerializeField] private Button _exitButton;
    
        private int _currentScore;
        private bool _isPlayerDead;

        private void Start()
        {
            Time.timeScale = 1.0f;
            PlayerEvents.OnGamePaused += ChangePauseState;
            EnemyEvents.OnDead += AddScore;
            gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            PlayerEvents.OnGamePaused -= ChangePauseState;
            EnemyEvents.OnDead -= AddScore;
        }

        private void OnEnable()
        {
            _continueButton.onClick.AddListener(ContinueButtonClick);
            _exitButton.onClick.AddListener(ExitButtonClick);
        }
        private void OnDisable()
        {
            _continueButton.onClick.RemoveAllListeners();
            _exitButton.onClick.RemoveAllListeners();
        }

        private void ChangePauseState(bool isPaused)
        {
            gameObject.SetActive(isPaused);
            if (isPaused)
            {
                Time.timeScale = 0f;
            }
            else
            {
                Time.timeScale = 1.0f;
            }
        }
    
        private void ContinueButtonClick()
        {
            PlayerEvents.PauseGame(false);
        }
        private void ExitButtonClick()
        {
            PlayerEvents.GameEnded(_currentScore);
            SceneManager.LoadScene("MainMenu");
        }
    
        private void AddScore()
        {
            _currentScore++;
        }

    }
}
